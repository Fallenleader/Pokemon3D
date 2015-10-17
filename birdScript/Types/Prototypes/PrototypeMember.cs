namespace birdScript.Types.Prototypes
{
    /// <summary>
    /// The member variable of a <see cref="Prototype"/>, with name and signature.
    /// </summary>
    class PrototypeMember
    {
        public string Identifier { get; }

        public SObject Data { get; set; }

        /// <summary>
        /// Determines if this member contains a function.
        /// </summary>
        internal bool IsFunction
        {
            get
            {
                return (Data != null && Data is SFunction);
            }
        }

        public PrototypeMember(string identifier, SObject data) : this(identifier, data, false, false, false, false) { }

        public PrototypeMember(string identifier, SObject data, bool isStatic, bool isReadOnly, bool isIndexerGet, bool isIndexerSet)
        {
            Identifier = identifier;
            Data = data;
            IsStatic = isStatic;
            IsReadOnly = isReadOnly;
            IsIndexerGet = isIndexerGet;
            IsIndexerSet = isIndexerSet;
        }

        /// <summary>
        /// Converts the <see cref="Data"/> to an <see cref="SFunction"/>.
        /// </summary>
        internal SFunction ToFunction()
        {
            return (SFunction)Data;
        }

        #region Signature

        internal bool IsStatic { get; }

        internal bool IsReadOnly { get; }

        internal bool IsIndexerGet { get; }

        internal bool IsIndexerSet { get; }

        #endregion
    }
}
