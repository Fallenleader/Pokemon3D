using birdScript.Types;

namespace birdScript.Adapters
{
    /// <summary>
    /// A callback method for the processor to check if an API class has a specific member.
    /// </summary>
    /// <param name="processor">The requesting processor.</param>
    /// <param name="className">The API class's name.</param>
    /// <param name="memberName">The name of the member.</param>
    /// <returns>If the member exists.</returns>
    public delegate bool DHasMember(ScriptProcessor processor, string className, string memberName);
    
    /// <summary>
    /// A callback method for the processor to get a member of the API class.
    /// </summary>
    /// <param name="processor">The requesting processor.</param>
    /// <param name="accessor">The accessor, which contains data that accesses the member. When used via class.member, it contains <see cref="string"/>.</param>
    /// <param name="isIndexer">If the member access has been done via an index example -> class[accessor]</param>
    /// <returns>The script object that the member of the API class contains.</returns>
    public delegate SObject DGetMember(ScriptProcessor processor, SObject accessor, bool isIndexer);

    /// <summary>
    /// A callback method for the processor to execute a method of an API class.
    /// </summary>
    /// <param name="processor">The requesting processor.</param>
    /// <param name="methodName">The name of the called method.</param>
    /// <param name="parameters">The parameters for this method call.</param>
    /// <returns>The result of the method call.</returns>
    public delegate SObject DExecuteMethod(ScriptProcessor processor, string methodName, SObject[] parameters);

    /// <summary>
    /// A callback method for the processor to set a member of an API class.
    /// </summary>
    /// <param name="processor">The processor which executes the script that called the setter.</param>
    /// <param name="accessor">The accessor, which contains data that accesses the member. When used via class.member, it contains <see cref="string"/>.</param>
    /// <param name="isIndexer">If the member access has been done via an index example -> class[accessor]</param>
    /// <param name="value">The value this member should be set to.</param>
    public delegate void DSetMember(ScriptProcessor processor, SObject accessor, bool isIndexer, SObject value);

    /// <summary>
    /// A callback method for the processor to get access to the source of another script file.
    /// </summary>
    /// <param name="processor">The source processor.</param>
    /// <param name="link">The defined link to the file.</param>
    public delegate string DScriptPipeline(ScriptProcessor processor, string link);
}
