using CumulusGame.Entity.Fertilizers;
using CumulusGame.Graphics;
using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CumulusGame.Entity
{
    public enum CumulusState
    {
        Waiting,
        SeekingTarget,
        GoingToTarget,
        EatingFertilizer,
        DestroyingRock
    }

    public class Cumulus : GameEntity
    {
        #region Fields

        // Constants
        private const float CUMULUS_SCALE = 0.52f;
        private const float BASE_MOOVE_SPEED = 2f;

        // Processing fields
        private Vector2 _velocity = Vector2.Zero;
        private Vector2 CenteredPosition => new Vector2(_hitbox.Center.X, _hitbox.Center.Y);

        private CumulusState State { get; set; } = CumulusState.Waiting;

        private float _moveSpeed;
        private float Anger { get; set; }
        private float _timeElapsedBetweenFrame;
        private float _timeCounter;
        private bool _animationEnabled;
        private bool _controlEnabled;

        private Cell _currentTargetedCell;
        private GameEntity _currentTarget;

        private readonly Animation _beginningAnimation;

        private Queue<GameEntity> Targets { get; } = new Queue<GameEntity>();
        private List<Rock> Obstacles { get; } = new List<Rock>();
        private Stack<Cell> Path { get; } = new Stack<Cell>();

        // Graphics Fields
        private const int NB_STATE_FRAME = 6;
        private const int NB_DIR_FRAME = 4;
        private const int FRAME_WIDTH = 195;
        private const int FRAME_HEIGHT = 400;
        private const int MILLIS_BETWEEN_FRAME = 75;

        private int _selectedStateFrame, _selectedDirFrame;

        #endregion  

        #region Constructors

        /// <summary>
        /// Constructor using texture & position
        /// </summary>
        /// <param name="position">The position</param>
        public Cumulus(Cell position)
        {
            switch (Utils.CurrentSkin)
            {
                case SkinType.CumulusBase:
                    _texture = Assets.Cumulus;
                    break;
                case SkinType.CumulusChat:
                    _texture = Assets.CumulusChat;
                    break;
                case SkinType.CumulusEgypt:
                    _texture = Assets.CumulusEgypt;
                    break;
                case SkinType.CumulusNina:
                    _texture = Assets.CumulusNina;
                    break;
                case SkinType.CumulusStValentin:
                    _texture = Assets.CumulusStValentin;
                    break;
                default:
                    _texture = Assets.Cumulus;
                    break;
            }

            _selectedStateFrame = 0;
            _selectedDirFrame = 3;

            _cellOn = position;
            _cellOn.IsEmpty = false;

            // Cumulus centered on position
            this._position = new Vector2((_cellOn.Position.X - (((float)FRAME_WIDTH / 2) * CUMULUS_SCALE)) + (Utils.CELL_SIZE / 2), (_cellOn.Position.Y - (((float)FRAME_HEIGHT / 2) * CUMULUS_SCALE)));

            _drawRectangle = new Rectangle((int)this._position.X, (int)this._position.Y,
                                            (int)(_texture.Width * CUMULUS_SCALE) / NB_STATE_FRAME, (int)(_texture.Height * CUMULUS_SCALE) / NB_DIR_FRAME);

            // Hitbox half height of the Cumulus
            _hitbox = new Rectangle((int)this._position.X, (int)(this._position.Y + Utils.CELL_SIZE), (int)Utils.CELL_SIZE, (int)Utils.CELL_SIZE);
            _hitboxTex = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.White);

            _beginningAnimation = new Animation(Position, Utils.TIME_BEFORE_GAME_START, 12, Assets.AnimationCumulusBeginning, TypeOfSheet.Horizontal, false, CUMULUS_SCALE);

            Anger = 0;
            _moveSpeed = BASE_MOOVE_SPEED + (Anger * 0.1f);
        }

        #endregion

        #region Private Methods

        private void StateMachine(GameTime gameTime)
        {
            switch (State)
            {
                case CumulusState.Waiting:
                    if (Targets.Count > 0)
                        State = CumulusState.SeekingTarget;
                    break;
                case CumulusState.SeekingTarget:
                    _currentTarget = Targets.Dequeue();
                    var nbCloseCell = _currentTarget.Cell.GetNeighbours().Count;
                    var nbNotEmptyCloseCell = GetNbNotEmptyCloseCells(_currentTarget.Cell);

                    var pathPossible = false;

                    if (_cellOn.CheckIfCellIsNear(_currentTarget.Cell))
                    {
                        State = _currentTarget is Fertilizer ? CumulusState.EatingFertilizer : CumulusState.DestroyingRock;
                        break;
                    }
                    if (nbNotEmptyCloseCell != nbCloseCell)
                        pathPossible = PathFinding(_cellOn, _currentTarget.Cell);
                    else
                    {
                        foreach (Cell cell in _currentTarget.Cell.GetNeighbours())
                        {
                            for (var i = Obstacles.Count - 1; i >= 0; i--)
                            {
                                if (Obstacles[i].Cell.Equals(cell))
                                {
                                    Targets.Enqueue(Obstacles[i]);
                                    Obstacles.Remove(Obstacles[i]);
                                    Targets.Enqueue(_currentTarget);
                                    State = CumulusState.Waiting;
                                    break;
                                }
                            }
                            if (State == CumulusState.Waiting)
                                break;
                        }
                    }

                    if (Path.Count > 0 && pathPossible)
                    {
                        _currentTargetedCell = Path.Peek();
                        State = CumulusState.GoingToTarget;
                    }
                    else
                    {
                        Rock tempObstacle = Obstacles[Utils.Random.Next(0, Obstacles.Count - 1)];
                        Targets.Enqueue(tempObstacle);
                        Obstacles.Remove(tempObstacle);
                        Targets.Enqueue(_currentTarget);
                        State = CumulusState.Waiting;
                    }
                    break;
                case CumulusState.GoingToTarget:
                    _timeElapsedBetweenFrame += gameTime.ElapsedGameTime.Milliseconds;
                    if (_timeElapsedBetweenFrame > MILLIS_BETWEEN_FRAME)
                    {
                        _timeElapsedBetweenFrame = 0;
                        if (_selectedStateFrame > 3)
                        {
                            _selectedStateFrame = 0;
                        }
                        else
                        {
                            _selectedStateFrame++;
                        }
                    }

                    if (Path.Count > 0)
                        FollowingThePath(gameTime.ElapsedGameTime.Milliseconds);
                    else
                    {
                        if (_currentTarget.Cell.CheckIfCellIsNear(_cellOn))
                            State = _currentTarget is Fertilizer
                                ? CumulusState.EatingFertilizer
                                : CumulusState.DestroyingRock;
                        else
                        {
                            Targets.Enqueue(_currentTarget);
                            State = CumulusState.SeekingTarget;
                        }
                    }
                    break;
                case CumulusState.EatingFertilizer:
                    var currentFerti = (Fertilizer)_currentTarget;

                    ChangeDirectionFrameToTarget(currentFerti);

                    // Le cumulus mange le cible (En fonction du temps que cette dernière met à être mangée)
                    Utils.TimeElapsedSinceStartEating += gameTime.ElapsedGameTime.Milliseconds;
                    if (Utils.TimeElapsedSinceStartEating > (currentFerti.TimeToEat - ((Anger * currentFerti.TimeToEat) / 100)))
                    {
                        FertilizerEating(currentFerti);
                    }
                    break;
                case CumulusState.DestroyingRock:
                    _selectedStateFrame = 5;
                    var currentRock = (Rock)_currentTarget;

                    ChangeDirectionFrameToTarget(currentRock);

                    // Le cumulus mange le cible (En fonction du temps que cette dernière met à être mangée)
                    Utils.TimeElapsedSinceStartBreaking += gameTime.ElapsedGameTime.Milliseconds;
                    if (Utils.TimeElapsedSinceStartBreaking > currentRock.TimeToBreak)
                    {
                        RockDestruction();
                    }
                    else
                    {
                        currentRock.Destroy();
                    }
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        private static int GetNbNotEmptyCloseCells(Cell currentCell)
        {
            var nbNotEmptyCloseCell = 0;
            foreach (Cell cell in currentCell.GetNeighbours())
            {
                if (!cell.IsEmpty)
                    nbNotEmptyCloseCell++;
            }
            return nbNotEmptyCloseCell;
        }

        private void FertilizerEating(Fertilizer fertilizer)
        {
            Utils.TimeElapsedSinceStartEating = 0;
            Anger += fertilizer.AmountOfAnger;
            fertilizer.Destroy();
            _currentTarget = null;
            _moveSpeed = BASE_MOOVE_SPEED + (Anger * 0.1f);
            State = CumulusState.Waiting;
        }

        private void RockDestruction()
        {
            Utils.TimeElapsedSinceStartBreaking = 0;
            _currentTarget = null;

            var tempList = new List<GameEntity>();
            for (var i = Targets.Count - 1; i >= 0; i--)
            {
                if (Targets.Peek() is Fertilizer)
                {
                    tempList.Add(Targets.Dequeue());
                }
                else
                {
                    Targets.Dequeue();
                }
            }

            foreach (GameEntity gameEntity in tempList)
            {
                Targets.Enqueue(gameEntity);
            }
            State = CumulusState.Waiting;
        }

        private void ChangeDirectionFrameToTarget(GameEntity target)
        {
            if (target.Cell.Column - Cell.Column < 0)
            {
                _selectedDirFrame = 0;
            }
            else if (target.Cell.Column - Cell.Column > 0)
            {
                _selectedDirFrame = 1;
            }
            else if (target.Cell.Row - Cell.Row < 0)
            {
                _selectedDirFrame = 2;
            }
            else
            {
                _selectedDirFrame = 3;
            }
        }

        private void SetInPathCells()
        {
            foreach (Cell cell in Path)
            {
                cell.InPath = true;
            }
        }

        private void UnsetInPathCells()
        {
            foreach (Cell cell in Path)
            {
                cell.InPath = false;
            }
        }

        /// <summary>
        /// An A* pathfinding algorithm to find the best way to starting cell to targetCell
        /// </summary>
        /// <param name="startingCell"></param>
        /// <param name="targetCell"></param>
        private bool PathFinding(Cell startingCell, Cell targetCell)
        {
            UnsetInPathCells();
            Path.Clear();
            var openList = new List<Cell>();
            var closedList = new List<Cell>() { startingCell };

            foreach (Cell cell in startingCell.GetNeighbours())
            {
                if (cell.IsEmpty)
                    openList.Add(cell);
            }

            if (openList.Count < 1)
                return false;

            Cell currentCell;
            do
            {
                currentCell = openList[0];
                foreach (Cell cell in openList)
                {
                    if (cell.GetFCost(startingCell, targetCell) < currentCell.GetFCost(startingCell, targetCell))
                        currentCell = cell;
                }

                closedList.Add(currentCell);
                foreach (Cell cell in currentCell.GetNeighbours())
                {
                    if (!closedList.Contains(cell) && !openList.Contains(cell) && cell.IsEmpty)
                    {
                        openList.Add(cell);
                    }
                }

                openList.Remove(currentCell);
            } while (openList.Count > 0 && !targetCell.CheckIfCellIsNear(currentCell));

            if (targetCell.CheckIfCellIsNear(closedList[closedList.Count - 1]))
            {
                closedList.Remove(_cellOn);
                Path.Push(closedList[closedList.Count - 1]);
                Cell currentPathCell = closedList[closedList.Count - 1];
                closedList.Remove(currentPathCell);
                while (!currentPathCell.CheckIfCellIsNear(_cellOn))
                {
                    for (var i = closedList.Count - 1; i >= 0; i--)
                    {
                        if (currentPathCell.CheckIfCellIsNear(closedList[i]))
                        {
                            Path.Push(closedList[i]);
                            currentPathCell = closedList[i];
                            closedList.Remove(currentPathCell);
                        }
                    }
                }
                SetInPathCells();
                return true;
            }

            return false;


        }

        /// <summary>
        /// Set the cell passed in param as current cell for the cumulus
        /// </summary>
        /// <param name="cell"></param>
        private void SetTheCellAsCurrent(Cell cell)
        {
            if (_cellOn != null && cell != null)
            {
                _cellOn.IsEmpty = true;
                _cellOn = cell;
                _cellOn.IsEmpty = false;
            }
            else if (cell != null)
            {
                _cellOn = cell;
                _cellOn.IsEmpty = false;
            }
        }

        /// <summary>
        /// Follow the path list.
        /// </summary>
        /// <param name="millis">GameTime elapsed in milliseconds</param>
        private void FollowingThePath(float millis)
        {
            if (_currentTargetedCell.Equals(_cellOn))
            {
                Path.Pop();
                _currentTargetedCell.InPath = false;
                _currentTargetedCell = Path.Count > 0 ? Path.Peek() : null;
            }
            else
            {
                _currentTargetedCell = Path.Peek();
            }
            GoToNearCell(millis);
        }

        /// <summary>
        /// Go to the nearest cell based on direction
        /// </summary>
        /// <param name="millis">GameTime elapsed in milliseconds</param>
        private void GoToNearCell(float millis)
        {
            if (_cellOn.CheckIfCellIsNear(_currentTargetedCell))
            {
                SetVelocityBasedOnTarget(_currentTargetedCell);
                Vector2 speed = new Vector2(_velocity.X, _velocity.Y) * _moveSpeed * millis;

                if (!CenteredPosition.X.Equals(_currentTargetedCell.CenteredPosition.X))
                {
                    _selectedDirFrame = speed.X < 0 ? 0 : 1;

                    _position.X += speed.X;

                    UpdateHitboxPosition();

                    var difPosX = _currentTargetedCell.CenteredPosition.X - CenteredPosition.X;

                    if (Math.Abs(difPosX) <= Math.Abs(speed.X))
                    {
                        _position.X += difPosX;
                    }
                }
                if (!CenteredPosition.Y.Equals(_currentTargetedCell.CenteredPosition.Y))
                {
                    _selectedDirFrame = speed.Y < 0 ? 2 : 3;

                    _position.Y += speed.Y;

                    UpdateHitboxPosition();

                    var difPosY = _currentTargetedCell.CenteredPosition.Y - CenteredPosition.Y;

                    if (Math.Abs(difPosY) <= Math.Abs(speed.Y))
                    {
                        _position.Y += difPosY;
                    }
                }
                if (CenteredPosition.Y.Equals(_currentTargetedCell.CenteredPosition.Y) && CenteredPosition.X.Equals(_currentTargetedCell.CenteredPosition.X))
                {
                    SetTheCellAsCurrent(_currentTargetedCell);
                }

                UpdateHitboxPosition();
            }
        }

        /// <summary>
        /// Set the velocity based on cell
        /// </summary>
        /// <param name="target">The cell targeted</param>
        private void SetVelocityBasedOnTarget(Cell target)
        {
            _velocity.X = ((target.CenteredPosition.X - CenteredPosition.X) / Math.Abs(target.CenteredPosition.X - CenteredPosition.X)) / 10;
            _velocity.Y = ((target.CenteredPosition.Y - CenteredPosition.Y) / Math.Abs(target.CenteredPosition.Y - CenteredPosition.Y)) / 10;
        }

        private void UpdateHitboxPosition()
        {
            _hitbox.X = (int)_position.X;
            _hitbox.Y = (int)(_position.Y + Utils.CELL_SIZE);
        }

        private void ClampInGameGrid()
        {
            if (_hitbox.Left < Utils.GameGrid.Left)
            {
                _position.X = Utils.GameGrid.Left;
            }
            if (_hitbox.Right > Utils.GameGrid.Right)
            {
                _position.X = Utils.GameGrid.Right - Utils.CELL_SIZE;
            }
            if (_hitbox.Top < Utils.GameGrid.Top)
            {
                _position.Y = Utils.GameGrid.Top;
            }
            if (_hitbox.Bottom > Utils.GameGrid.Bottom)
            {
                _position.Y = Utils.GameGrid.Bottom - Utils.CELL_SIZE;
            }

            UpdateHitboxPosition();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update the path
        /// </summary>
        public void UpdatePath()
        {
            PathFinding(_currentTargetedCell, _currentTarget.Cell);
        }

        /// <summary>
        /// Add a target to the target list of the cumulus
        /// </summary>
        /// <param name="target">The target</param>
        public void AddTarget(GameEntity target)
        {
            Targets.Enqueue(target);
            if (Path.Contains(target.Cell))
            {
                UpdatePath();
            }
        }

        /// <summary>
        /// Add an obstacle to the potentialObstacle list of the cumulus
        /// </summary>
        /// <param name="rock">The rock</param>
        public void AddObstacle(Rock rock)
        {
            Obstacles.Add(rock);
            if (Path.Contains(rock.Cell))
            {
                UpdatePath();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_controlEnabled)
            {
                spriteBatch.Draw(_texture, _position,
                    new Rectangle(_selectedStateFrame * FRAME_WIDTH, _selectedDirFrame * FRAME_HEIGHT, FRAME_WIDTH,
                        FRAME_HEIGHT),
                    Color.White, 0f, Vector2.Zero, CUMULUS_SCALE, SpriteEffects.None, Utils.ENTITY_DEPTH);
            }
            else
            {

                _beginningAnimation.Draw(spriteBatch);
            }

            //spriteBatch.Draw(hitboxTex, hitbox, Color.Red);
        }

        public override void Update(GameTime gameTime)
        {
            if (_controlEnabled)
            {
                // Gestion du comportement du Cumulus
                StateMachine(gameTime);

                // Bloque le Cumulus sur la grille de jeu
                ClampInGameGrid();

                // Met à jour la hitbox du Cumulus
                UpdateHitboxPosition();
            }
            else
            {
                _timeCounter += gameTime.ElapsedGameTime.Milliseconds;
                if (_timeCounter > Utils.TIME_BEFORE_ANIMATION)
                {
                    _timeCounter = 0;
                    _animationEnabled = true;
                }

                if (_animationEnabled)
                {
                    _beginningAnimation.Play(gameTime.ElapsedGameTime.Milliseconds);
                }

                if (_beginningAnimation.AnimationEnded)
                {
                    _controlEnabled = true;
                }
            }


        }

        #endregion
    }
}
