using CumulusGame.Entity;
using CumulusGame.Entity.Fertilizers;
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
        private Cell[,] _cells;

        public int Width { get; }
        public int Height { get; }
        public int NbVerticalCell { get; }
        public int NbHorizontalCell { get; }

        private Vector2 _position;

        // Graphics fields
        private Rectangle _hitbox;
        private Texture2D _hitboxTex;

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
            Width = Utils.Window_Width;
            Height = (int)(Utils.Window_Height * 0.8f);
            NbHorizontalCell = (int)(Width / Utils.CELL_SIZE);
            NbVerticalCell = (int)(Height / Utils.CELL_SIZE);
            _position = new Vector2(((float)Width / NbHorizontalCell) - (Utils.CELL_SIZE / 2), Utils.GameBoardOffset + (((float)Height / NbVerticalCell) - (Utils.CELL_SIZE / 2)));
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
                _position.Y = Utils.GameBoardOffset + (((float)Height / NbVerticalCell) - (Utils.CELL_SIZE / 2));
                _position.X += Utils.CELL_SIZE;
            }

            _position = new Vector2(((float)Width / NbHorizontalCell) - Utils.CELL_SIZE, Utils.GameBoardOffset + (Height / NbVerticalCell) - Utils.CELL_SIZE);

            _hitbox = new Rectangle((int)_position.X, (int)_position.Y, Width, Height);
            _hitboxTex = Utils.CreateContouringRectangleTexture(_hitbox.Width, _hitbox.Height, Color.Red);
        }

        #endregion

        #region Public Methods

        public bool CellIsInTheGrid(Cell cell)
        {
            return (cell.Column >= 0 && cell.Column < NbHorizontalCell && cell.Row >= 0 && cell.Row < NbVerticalCell);
        }

        public bool CellIsInTheGrid(int column, int row)
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

        public void CreateRockOnCell(MouseState mouse, List<Rock> rocks, List<GameEntity> gameEntities)
        {
            foreach (var cell in _cells)
            {
                if (Input.KeyPressed(Keys.Space, true))
                {
                    if (cell.Hovered)
                    {
                        if (cell.IsEmpty)
                        {
                            Utils.CreateRock(cell,
                                rocks, gameEntities);
                        }
                    }
                }
            }
        }

        public void CreateObjectOnCell(MouseState mouse, List<Fertilizer> fertilizers, List<Rock> rocks, List<GameEntity> gameEntities)
        {
            foreach (var cell in _cells)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (cell.Hovered)
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
                                    throw new ArgumentOutOfRangeException();
                            }
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
            foreach (var cell in _cells)
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
            foreach (var cell in _cells)
            {
                cell.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var cell in _cells)
            {
                cell.Draw(spriteBatch);
            }
            //spriteBatch.Draw(_hitboxTex, _hitbox, Color.White);
        }

        #endregion
    }
}
