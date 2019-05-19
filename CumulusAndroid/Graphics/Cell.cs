using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CumulusGame.Graphics
{
    public class Cell
    {
        #region Fields

        private const float HoverAlpha = 0.2f;
        public float Size { get; }

        public bool InPath { get; set; } = false;
        public bool IsEmpty { get; set; } = true;
        public bool Hovered { get; private set; }

        public int Row { get; }
        public int Column { get; }

        public Vector2 Position { get; }
        public Vector2 CenteredPosition => new Vector2(Position.X + (Size / 2), Position.Y + (Size / 2));

        public Rectangle Hitbox { get; }

        private Texture2D _texture;
        private Texture2D _contouringTexture;

        private float _alpha = 1f;

        #endregion

        #region Constructor

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="size">The size of the cell</param>
        /// <param name="x">The X coord of the center</param>
        /// <param name="y">The Y coord of the center</param>
        /// <param name="row">The rank in the grid's row</param>
        /// <param name="column">The rank in the grid's column</param>
        public Cell(float size, int x, int y, int row, int column)
        {
            Size = size;
            Row = row;
            Column = column;

            Position = new Vector2(x - (Size / 2), y - (Size / 2));

            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Size, (int)Size);

            _texture = Assets.Cell;
            _contouringTexture = Utils.CreateContouringRectangleTexture(Hitbox.Width, Hitbox.Height, Color.DarkGray);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="size">The size of the cell</param>
        /// <param name="x">The X coord of the center</param>
        /// <param name="y">The Y coord of the center</param>
        /// <param name="row">The rank in the grid's row</param>
        /// <param name="column">The rank in the grid's column</param>
        /// <param name="alpha">The transparency of the cell's texture</param>
        public Cell(float size, int x, int y, int row, int column, float alpha)
        {
            Size = size;
            Row = row;
            Column = column;

            Position = new Vector2(x - (Size / 2), y - (Size / 2));

            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, (int)Size, (int)Size);

            _texture = Utils.CreateRectangleTexture(Hitbox.Width, Hitbox.Height, Color.White);
            _contouringTexture = Utils.CreateContouringRectangleTexture(Hitbox.Width, Hitbox.Height, Color.DarkGray);
            _alpha = alpha;
        }

        #endregion

        #region Private Methods

        private int CalculateDistance(Cell referenceCell)
        {
            var cost = (Math.Abs(referenceCell.Column - Column) + Math.Abs(referenceCell.Row - Row));
            return cost;
        }

        public bool CheckIfCoordInCell(Vector2 position)
        {
            return (position.X >= Hitbox.Left && position.X <= Hitbox.Right && position.Y >= Hitbox.Top && position.Y <= Hitbox.Bottom);
        }

        #endregion

        #region Public Methods

        public int GetFCost(Cell startingCell, Cell targetCell)
        {
            return CalculateDistance(startingCell) + CalculateDistance(targetCell);
        }

        public int GetHCost(Cell targetCell)
        {
            return CalculateDistance(targetCell);
        }

        public int GetGCost(Cell startingCell)
        {
            return CalculateDistance(startingCell);
        }

        public List<Cell> GetNeighbours()
        {
            var neighbours = new List<Cell>();
            var tempCell = Utils._gameGrid.GetCellByRowColumn(Column, Row - 1);
            if (tempCell != null)
            {
                neighbours.Add(tempCell);
            }

            tempCell = Utils._gameGrid.GetCellByRowColumn(Column + 1, Row);
            if (tempCell != null)
            {
                neighbours.Add(tempCell);
            }

            tempCell = Utils._gameGrid.GetCellByRowColumn(Column, Row + 1);
            if (tempCell != null)
            {
                neighbours.Add(tempCell);
            }

            tempCell = Utils._gameGrid.GetCellByRowColumn(Column - 1, Row);
            if (tempCell != null)
            {
                neighbours.Add(tempCell);
            }

            return neighbours;
        }

        /// <summary>
        /// Check if the cell is near the current cell
        /// </summary>
        /// <param name="cell">The cell we want to verify</param>
        /// <returns></returns>
        public bool CheckIfCellIsNear(Cell cell)
        {
            return cell?.GetNeighbours().Contains(this) ?? false;
        }

        /// <summary>
        /// Set the cell to hovered if the mouse is on it
        /// </summary>
        /// <param name="mouse">The mouse</param>
        private void SetHoveredIfMouseOnCell(MouseState mouse)
        {
            Hovered = CheckIfCoordInCell(new Vector2(mouse.X, mouse.Y));
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            SetHoveredIfMouseOnCell(mouse);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Hovered)
            {
                spriteBatch.Draw(_texture, Position, null, Color.Red * HoverAlpha, 0f, Vector2.Zero, 1, SpriteEffects.None, Utils.VISUALHELPER_DEPTH);
                spriteBatch.Draw(_contouringTexture, Position, null, Color.Red * HoverAlpha, 0f, Vector2.Zero, 1, SpriteEffects.None, Utils.VISUALHELPER_DEPTH);
            }
            else if (InPath)
            {
                spriteBatch.Draw(_texture, Position, null, Color.White * _alpha, 0f, Vector2.Zero, 1, SpriteEffects.None, Utils.VISUALHELPER_DEPTH);

                spriteBatch.Draw(_texture, Position, null, Color.Red * HoverAlpha, 0f, Vector2.Zero, 1, SpriteEffects.None, Utils.VISUALHELPER_DEPTH);
                spriteBatch.Draw(_contouringTexture, Position, null, Color.Blue, 0f, Vector2.Zero, 1, SpriteEffects.None, Utils.VISUALHELPER_DEPTH);
            }
            else
            {
                spriteBatch.Draw(_texture, Position, null, Color.White * _alpha, 0f, Vector2.Zero, 1, SpriteEffects.None, Utils.VISUALHELPER_DEPTH);
            }
        }

        #endregion
    }
}
