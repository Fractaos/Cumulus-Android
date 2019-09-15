using CumulusGame.Entity;
using CumulusGame.Entity.Fertilizers;
using CumulusGame.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CumulusGame.Graphics
{
    public class Grid
    {
        #region Fields

        // Processing fields
        private readonly Cell[,] _cells;

        private int Width { get; }
        private int Height { get; }
        private int NbVerticalCell { get; }
        private int NbHorizontalCell { get; }

        private readonly Vector2 _position;

        // Graphics fields
#if DEBUG
        private readonly Rectangle _hitbox;
        private readonly Texture2D _hitboxTex;
#endif

        public int Left => 0;
        public int Bottom => Height + (int)_position.Y;
        public int Top => (int)_position.Y;
        public int Right => Width;

        #endregion

        #region Constructors

        /// <summary>
        /// The constructor
        /// </summary>
        public Grid()
        {
            Width = Utils.SCREEN_WIDTH;
            Height = (int)(Utils.SCREEN_HEIGHT * 0.8f);
            NbHorizontalCell = (int)(Width / Utils.CELL_SIZE);
            NbVerticalCell = (int)(Height / Utils.CELL_SIZE);
            _position = new Vector2(((float)Width / NbHorizontalCell) - (Utils.CELL_SIZE / 2), Utils.GAMEBOARD_OFFSET + (((float)Height / NbVerticalCell) - (Utils.CELL_SIZE / 2)));
            _cells = new Cell[NbHorizontalCell, NbVerticalCell];

            for (var i = 0; i < NbHorizontalCell; i++)
            {
                for (var j = 0; j < NbVerticalCell; j++)
                {
                    if ((j - (i % 2)) % 2 == 0)
                    {
                        _cells[i, j] = new Cell(Utils.CELL_SIZE, (int)_position.X, (int)_position.Y, j, i, 0.2f);
                    }
                    else
                    {
                        _cells[i, j] = new Cell(Utils.CELL_SIZE, (int)_position.X, (int)_position.Y, j, i, 0);
                    }
                    _position.Y += Utils.CELL_SIZE;
                }
                _position.Y = Utils.GAMEBOARD_OFFSET + (((float)Height / NbVerticalCell) - (Utils.CELL_SIZE / 2));
                _position.X += Utils.CELL_SIZE;
            }

            _position = new Vector2(((float)Width / NbHorizontalCell) - Utils.CELL_SIZE, Utils.GAMEBOARD_OFFSET + (Height / NbVerticalCell) - Utils.CELL_SIZE);
#if DEBUG
            _hitbox = new Rectangle((int)_position.X, (int)_position.Y, Width, Height);
            _hitboxTex = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
#endif
        }

        #endregion

        #region Public Methods

        public bool CellIsInTheGrid(Cell cell)
        {
            return (cell.Column >= 0 && cell.Column < NbHorizontalCell && cell.Row >= 0 && cell.Row < NbVerticalCell);
        }

        private bool CellIsInTheGrid(int column, int row)
        {
            return (column >= 0 && column < NbHorizontalCell && row >= 0 && row < NbVerticalCell);
        }

        /// <summary>
        /// Get a Cell by her coord
        /// </summary>
        /// <param name="position">The position of the cell</param>
        /// <returns></returns>
        public Cell GetCellByCoord(Vector2 position)
        {
            return _cells.Cast<Cell>().FirstOrDefault(cell => cell.CheckIfCoordInCell(position));
        }

        public Cell GetCellByRowColumn(int column, int row)
        {
            return CellIsInTheGrid(column, row) ? _cells[column, row] : null;
        }

        public void CreateObjectOnCell(List<Fertilizer> fertilizers, List<Rock> rocks, List<GameEntity> gameEntities)
        {
            if (Input.OneTouched())
            {
                Cell cell = GetCellByCoord(Input.FirstTouchPosition);
                if (cell != null && cell.Hovered)
                {
                    if (cell.IsEmpty)
                    {
                        switch (Utils.CurrentObjectSelected)
                        {
                            case UsableObject.Rock:

                                Utils.CreateRock(cell,
                                    rocks, gameEntities);
                                break;
                            case UsableObject.LittleFertilizer:
                                Utils.CreateLittleFertilizer(new Vector2(cell.Position.X + (cell.Size / 2),
                                        cell.Position.Y +
                                        (cell.Size / 2)),
                                    fertilizers, gameEntities);
                                break;
                            case UsableObject.MediumFertilizer:
                                Utils.CreateMediumFertilizer(new Vector2(cell.Position.X + (cell.Size / 2),
                                        cell.Position.Y +
                                        (cell.Size / 2)),
                                    fertilizers, gameEntities);
                                break;
                            case UsableObject.LargeFertilizer:
                                Utils.CreateLargeFertilizer(new Vector2(cell.Position.X + (cell.Size / 2),
                                        cell.Position.Y +
                                        (cell.Size / 2)),
                                    fertilizers, gameEntities);
                                break;
                            default:
                                throw new IndexOutOfRangeException();
                        }
                    }
                }
            }

        }
        /// <summary>
        /// Create a fertilizer on the cell under the mouse
        /// The fertilizer created depends on the button of the mouse pressed
        /// </summary>
        /// <param name="mouse">The mouse</param>
        /// <param name="fertilizers">The fertilizers list where we add the fertilizer</param>
        /// <param name="gameEntities">The game entities list where we add the fertilizer</param>
        public void CreateFertilizerOnCell(MouseState mouse, List<Fertilizer> fertilizers, List<GameEntity> gameEntities)
        {
            foreach (Cell cell in _cells)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (cell.Hovered)
                    {
                        if (cell.IsEmpty)
                        {
                            Utils.CreateLittleFertilizer(new Vector2(cell.Position.X + (cell.Size / 2), cell.Position.Y +
                                                                                                        (cell.Size / 2)),
                                fertilizers, gameEntities);
                        }
                    }
                }
                if (mouse.RightButton == ButtonState.Pressed)
                {
                    if (cell.Hovered)
                    {
                        if (cell.IsEmpty)
                        {
                            Utils.CreateMediumFertilizer(new Vector2(cell.Position.X + (cell.Size / 2), cell.Position.Y +
                                                                                                        (cell.Size / 2)),
                                fertilizers, gameEntities);
                        }
                    }
                }
                if (mouse.MiddleButton == ButtonState.Pressed)
                {
                    if (cell.Hovered)
                    {
                        if (cell.IsEmpty)
                        {
                            Utils.CreateLargeFertilizer(new Vector2(cell.Position.X + (cell.Size / 2), cell.Position.Y +
                                                                                                       (cell.Size / 2)),
                                fertilizers, gameEntities);
                        }
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Cell cell in _cells)
            {
                cell.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cell cell in _cells)
            {
                cell.Draw(spriteBatch);
            }
#if DEBUG
            spriteBatch.Draw(_hitboxTex, _hitbox, Color.White);
#endif
        }

        #endregion
    }
}
