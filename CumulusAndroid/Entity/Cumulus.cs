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
        private const float CumulusScale = Utils.SCALE * 0.52f;
        private const float BaseMooveSpeed = Utils.SCALE * 2f;

        // Processing fields
        private Vector2 _velocity = Vector2.Zero;
        private Vector2 CenteredPosition => new Vector2(hitbox.Center.X, hitbox.Center.Y);

        public CumulusState State { get; private set; } = CumulusState.Waiting;

        private float _moveSpeed;
        public float Anger { get; private set; }
        private float _timeElapsedBetweenFrame;
        private float _timeCounter;
        private bool _animationEnabled;
        private bool _controlEnabled;

        private Cell _currentTargetedCell;
        private GameEntity _currentTarget;

        private readonly Animation _beginningAnimation;

        public Queue<GameEntity> Targets { get; } = new Queue<GameEntity>();
        public List<Rock> Obstacles { get; } = new List<Rock>();
        public Stack<Cell> Path { get; } = new Stack<Cell>();

        // Graphics Fields
        private const int NbStateFrame = 6;
        private const int NbDirFrame = 4;
        private const int FrameWidth = 195;
        private const int FrameHeight = 400;
        private const int MillisBetweenFrame = 75;

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
                    texture = Assets.Cumulus;
                    break;
                case SkinType.CumulusChat:
                    texture = Assets.CumulusChat;
                    break;
                case SkinType.CumulusEgypt:
                    texture = Assets.CumulusEgypt;
                    break;
                case SkinType.CumulusNina:
                    texture = Assets.CumulusNina;
                    break;
                case SkinType.CumulusStValentin:
                    texture = Assets.CumulusStValentin;
                    break;
                default:
                    texture = Assets.Cumulus;
                    break;
            }

            _selectedStateFrame = 0;
            _selectedDirFrame = 3;

            cellOn = position;
            cellOn.IsEmpty = false;

            // Cumulus centered on position
            this.position = new Vector2((cellOn.Position.X - (((float)FrameWidth / 2) * CumulusScale)) + (Utils.CELL_SIZE / 2), (cellOn.Position.Y - (((float)FrameHeight / 2) * CumulusScale)));

            drawRectangle = new Rectangle((int)this.position.X, (int)this.position.Y,
                                            (int)(texture.Width * CumulusScale) / NbStateFrame, (int)(texture.Height * CumulusScale) / NbDirFrame);

            // Hitbox half height of the Cumulus
            hitbox = new Rectangle((int)this.position.X, (int)(this.position.Y + Utils.CELL_SIZE), (int)Utils.CELL_SIZE, (int)Utils.CELL_SIZE);
            hitboxTex = Utils.CreateContouringRectangleTexture(hitbox.Width, hitbox.Height, Color.White);

            _beginningAnimation = new Animation(Position, Utils.TIME_BEFORE_GAME_START, 12, Assets.AnimationCumulusBeginning, TypeOfSheet.Horizontal, false, CumulusScale);

            Anger = 0;
            _moveSpeed = BaseMooveSpeed + (Anger * 0.1f);
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

                    if (cellOn.CheckIfCellIsNear(_currentTarget.Cell))
                    {
                        State = _currentTarget is Fertilizer ? CumulusState.EatingFertilizer : CumulusState.DestroyingRock;
                        break;
                    }
                    if (nbNotEmptyCloseCell != nbCloseCell)
                        pathPossible = PathFinding(cellOn, _currentTarget.Cell);
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
                        var tempObstacle = Obstacles[Utils.Random.Next(0, Obstacles.Count - 1)];
                        Targets.Enqueue(tempObstacle);
                        Obstacles.Remove(tempObstacle);
                        Targets.Enqueue(_currentTarget);
                        State = CumulusState.Waiting;
                    }
                    break;
                case CumulusState.GoingToTarget:
                    _timeElapsedBetweenFrame += gameTime.ElapsedGameTime.Milliseconds;
                    if (_timeElapsedBetweenFrame > MillisBetweenFrame)
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
                        if (_currentTarget.Cell.CheckIfCellIsNear(cellOn))
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
                    Utils.timeElapsedSinceStartEating += gameTime.ElapsedGameTime.Milliseconds;
                    if (Utils.timeElapsedSinceStartEating > (currentFerti.TimeToEat - ((Anger * currentFerti.TimeToEat) / 100)))
                    {
                        FertilizerEating(currentFerti);
                    }
                    break;
                case CumulusState.DestroyingRock:
                    _selectedStateFrame = 5;
                    var currentRock = (Rock)_currentTarget;

                    ChangeDirectionFrameToTarget(currentRock);

                    // Le cumulus mange le cible (En fonction du temps que cette dernière met à être mangée)
                    Utils.timeElapsedSinceStartBreaking += gameTime.ElapsedGameTime.Milliseconds;
                    if (Utils.timeElapsedSinceStartBreaking > currentRock.TimeToBreak)
                    {
                        RockDestruction();
                    }
                    else
                    {
                        currentRock.Destroy();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static int GetNbNotEmptyCloseCells(Cell currentCell)
        {
            var nbNotEmptyCloseCell = 0;
            foreach (var cell in currentCell.GetNeighbours())
            {
                if (!cell.IsEmpty)
                    nbNotEmptyCloseCell++;
            }
            return nbNotEmptyCloseCell;
        }

        private void FertilizerEating(Fertilizer fertilizer)
        {
            Utils.timeElapsedSinceStartEating = 0;
            Anger += fertilizer.AmountOfAnger;
            fertilizer.Destroy();
            _currentTarget = null;
            _moveSpeed = BaseMooveSpeed + (Anger * 0.1f);
            State = CumulusState.Waiting;
        }

        private void RockDestruction()
        {
            Utils.timeElapsedSinceStartBreaking = 0;
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

            foreach (var gameEntity in tempList)
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
            foreach (var cell in Path)
            {
                cell.InPath = true;
            }
        }

        private void UnsetInPathCells()
        {
            foreach (var cell in Path)
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

            foreach (var cell in startingCell.GetNeighbours())
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
                foreach (var cell in openList)
                {
                    if (cell.GetFCost(startingCell, targetCell) < currentCell.GetFCost(startingCell, targetCell))
                        currentCell = cell;
                }

                closedList.Add(currentCell);
                foreach (var cell in currentCell.GetNeighbours())
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
                closedList.Remove(cellOn);
                Path.Push(closedList[closedList.Count - 1]);
                var currentPathCell = closedList[closedList.Count - 1];
                closedList.Remove(currentPathCell);
                while (!currentPathCell.CheckIfCellIsNear(cellOn))
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
            if (cellOn != null && cell != null)
            {
                cellOn.IsEmpty = true;
                cellOn = cell;
                cellOn.IsEmpty = false;
            }
            else if (cell != null)
            {
                cellOn = cell;
                cellOn.IsEmpty = false;
            }
        }

        /// <summary>
        /// Follow the path list.
        /// </summary>
        /// <param name="millis">GameTime elapsed in milliseconds</param>
        private void FollowingThePath(float millis)
        {
            if (_currentTargetedCell.Equals(cellOn))
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
            if (cellOn.CheckIfCellIsNear(_currentTargetedCell))
            {
                SetVelocityBasedOnTarget(_currentTargetedCell);
                Vector2 speed = new Vector2(_velocity.X, _velocity.Y) * _moveSpeed * millis;

                if (!CenteredPosition.X.Equals(_currentTargetedCell.CenteredPosition.X))
                {
                    _selectedDirFrame = speed.X < 0 ? 0 : 1;

                    position.X += speed.X;

                    UpdateHitboxPosition();

                    var difPosX = _currentTargetedCell.CenteredPosition.X - CenteredPosition.X;

                    if (Math.Abs(difPosX) <= Math.Abs(speed.X))
                    {
                        position.X += difPosX;
                    }
                }
                if (!CenteredPosition.Y.Equals(_currentTargetedCell.CenteredPosition.Y))
                {
                    _selectedDirFrame = speed.Y < 0 ? 2 : 3;

                    position.Y += speed.Y;

                    UpdateHitboxPosition();

                    var difPosY = _currentTargetedCell.CenteredPosition.Y - CenteredPosition.Y;

                    if (Math.Abs(difPosY) <= Math.Abs(speed.Y))
                    {
                        position.Y += difPosY;
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
            hitbox.X = (int)position.X;
            hitbox.Y = (int)(position.Y + Utils.CELL_SIZE);
        }

        private void ClampInGameGrid()
        {
            if (hitbox.Left < Utils._gameGrid.Left)
            {
                position.X = Utils._gameGrid.Left;
            }
            if (hitbox.Right > Utils._gameGrid.Right)
            {
                position.X = Utils._gameGrid.Right - Utils.CELL_SIZE;
            }
            if (hitbox.Top < Utils._gameGrid.Top)
            {
                position.Y = Utils._gameGrid.Top;
            }
            if (hitbox.Bottom > Utils._gameGrid.Bottom)
            {
                position.Y = Utils._gameGrid.Bottom - Utils.CELL_SIZE;
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
                spriteBatch.Draw(texture, position,
                    new Rectangle(_selectedStateFrame * FrameWidth, _selectedDirFrame * FrameHeight, FrameWidth,
                        FrameHeight),
                    Color.White, 0f, Vector2.Zero, CumulusScale, SpriteEffects.None, Utils.ENTITY_DEPTH);
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
