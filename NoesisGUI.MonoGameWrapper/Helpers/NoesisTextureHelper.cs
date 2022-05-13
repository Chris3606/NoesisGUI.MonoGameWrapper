namespace NoesisGUI.MonoGameWrapper.Helpers
{
    using System;
    using System.Reflection;
    using Texture = Noesis.Texture;
    using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

    public static class NoesisTextureHelper
    {
        // We need the native texture pointer for the OpenGL texture
        // but this API is not available in MonoGame.Texture class.
        // Here we're using reflection to access the internal field.
        private static readonly FieldInfo GetTextureField
            = typeof(Texture2D).GetField("glTexture",
                                          BindingFlags.NonPublic | BindingFlags.Instance);

        public static Texture CreateNoesisTexture(Texture2D texture)
        {
            if (texture == null)
            {
                return null;
            }

            if (texture.IsDisposed)
            {
                return null;
            }

            var textureNativePointer = GetTextureNativePointer(texture);

            return Texture.WrapD3D11Texture(
                texture,
                textureNativePointer,
                texture.Width,
                texture.Height,
                texture.LevelCount,
                isInverted: false);
        }

        private static IntPtr GetTextureNativePointer(Texture2D texture)
            => new IntPtr((int)GetTextureField.GetValue(texture));
    }
}