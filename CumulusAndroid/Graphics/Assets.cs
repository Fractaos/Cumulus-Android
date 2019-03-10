using Microsoft.Xna.Framework.Graphics;

namespace CumulusAndroid.Graphics
{
    public enum SkinType
    {
        CumulusBase,
        CumulusEgypt,
        CumulusChat,
        CumulusStValentin,
        CumulusNina
    }

    public class Assets
    {
        #region Fields

        public static Texture2D LittleFertilizer, MediumFertilizer, LargeFertilizer, Background, Cell, BackgroundSkinScreen,
            Rock, Lightning,
            MenuBg1, MenuBg2, MenuBg3, MenuBg4, MenuBg5, MenuBg6, MenuBg7, MenuBg8, MenuBg9, MenuBg10, MenuBg11,
            Button, ButtonSettings, ButtonArrow, MenuText, CooldownCircle,
            Cumulus, CumulusStValentin, CumulusFille, CumulusNina, CumulusEgypt, CumulusChat,
            AnimationCumulusBeginning,
            Isle,
            IconeLittleFertilizer, IconeMediumFertilizer, IconeLargeFertilizer, IconeRock;

        public static Effect EffectLittle, EffectMedium, EffectLarge, EffectRock;

        public static SpriteFont Pixel12, Pixel18, Pixel24, Pixel30, Pixel36;

        #endregion

        #region Public Methods

        public static void LoadAll()
        {
            Cumulus = Main.Content.Load<Texture2D>("graphics/cumulus");
            CumulusStValentin = Main.Content.Load<Texture2D>("graphics/cumulus-st_valentin");
            CumulusFille = Main.Content.Load<Texture2D>("graphics/cumulus-fille");
            CumulusNina = Main.Content.Load<Texture2D>("graphics/cumulus-nina");
            CumulusEgypt = Main.Content.Load<Texture2D>("graphics/cumulus-egypt");
            CumulusChat = Main.Content.Load<Texture2D>("graphics/cumulus-chat");

            //AnimationCumulusBeginning = Main.Content.Load<Texture2D>("graphics/animation_cumulus_beginning");

            //Background = Main.Content.Load<Texture2D>("graphics/background");
            //BackgroundSkinScreen = Main.Content.Load<Texture2D>("graphics/background_skin");
            //Cell = Main.Content.Load<Texture2D>("graphics/cell");

            //MenuBg1 = Main.Content.Load<Texture2D>("graphics/menu-bg_1");
            //MenuBg2 = Main.Content.Load<Texture2D>("graphics/menu-bg_2");
            //MenuBg3 = Main.Content.Load<Texture2D>("graphics/menu-bg_3");
            //MenuBg4 = Main.Content.Load<Texture2D>("graphics/menu-bg_4");
            //MenuBg5 = Main.Content.Load<Texture2D>("graphics/menu-bg_5");
            //MenuBg6 = Main.Content.Load<Texture2D>("graphics/menu-bg_6");
            //MenuBg7 = Main.Content.Load<Texture2D>("graphics/menu-bg_7");
            //MenuBg8 = Main.Content.Load<Texture2D>("graphics/menu-bg_8");
            //MenuBg9 = Main.Content.Load<Texture2D>("graphics/menu-bg_9");
            //MenuBg10 = Main.Content.Load<Texture2D>("graphics/menu-bg_10");
            //MenuBg11 = Main.Content.Load<Texture2D>("graphics/menu-bg_11");

            //Button = Main.Content.Load<Texture2D>("graphics/skin_button");
            //ButtonSettings = Main.Content.Load<Texture2D>("graphics/skin_button_settings");
            //ButtonArrow = Main.Content.Load<Texture2D>("graphics/skin_button_arrow");
            //MenuText = Main.Content.Load<Texture2D>("graphics/menu_text");
            //CooldownCircle = Main.Content.Load<Texture2D>("graphics/blank");

            //Rock = Main.Content.Load<Texture2D>("graphics/rock");
            //Lightning = Main.Content.Load<Texture2D>("graphics/light-animation");

            //LittleFertilizer = Main.Content.Load<Texture2D>("graphics/fertilizer_little");
            //MediumFertilizer = Main.Content.Load<Texture2D>("graphics/fertilizer_medium");
            //LargeFertilizer = Main.Content.Load<Texture2D>("graphics/fertilizer_large");

            //Isle = Main.Content.Load<Texture2D>("graphics/isle_skin");

            //EffectLittle = Main.Content.Load<Effect>("shaders/effectLittle");
            //EffectMedium = Main.Content.Load<Effect>("shaders/effectMedium");
            //EffectLarge = Main.Content.Load<Effect>("shaders/effectLarge");
            //EffectRock = Main.Content.Load<Effect>("shaders/effectRock");

            //IconeLittleFertilizer = Main.Content.Load<Texture2D>("graphics/icone-fertilizer_little");
            //IconeMediumFertilizer = Main.Content.Load<Texture2D>("graphics/icone-fertilizer_medium");
            //IconeLargeFertilizer = Main.Content.Load<Texture2D>("graphics/icone-fertilizer_large");
            //IconeRock = Main.Content.Load<Texture2D>("graphics/icone-rock");

            //Pixel12 = Main.Content.Load<SpriteFont>("fonts/pixel_12");
            //Pixel18 = Main.Content.Load<SpriteFont>("fonts/pixel_18");
            //Pixel24 = Main.Content.Load<SpriteFont>("fonts/pixel_24");
            //Pixel30 = Main.Content.Load<SpriteFont>("fonts/pixel_30");
            //Pixel36 = Main.Content.Load<SpriteFont>("fonts/pixel_36");
        }

        #endregion
    }
}
