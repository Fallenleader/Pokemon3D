namespace Pokemon3D.GameModes.Maps.EntityComponents
{
    /// <summary>
    /// The result type of the render method call of an <see cref="EntityComponent"/>.
    /// </summary>
    enum RenderResultType
    {
        /// <summary>
        /// This entity component rendered something and was the last component in this entity to render something.
        /// </summary>
        Rendered,
        /// <summary>
        /// This entity component rendered something, but the next component might also render something.
        /// </summary>
        RenderedButPassed,
        /// <summary>
        /// This entity component did not render something.
        /// </summary>
        Passed
    }
}
