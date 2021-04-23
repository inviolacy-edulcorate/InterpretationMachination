namespace InterpretationMachination.Interfaces.Interfaces
{
    /// <summary>
    /// Interface describing an interpreter which produces a result of type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">Produced result type.</typeparam>
    public interface IInterpreter<out T>
    {
        /// <summary>
        /// Interpret the <paramref name="inputString"/> and produce a result of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="inputString">The input string to interpret.</param>
        /// <returns>The interpreted result.</returns>
        T Interpret(string inputString);
    }

    /// <summary>
    /// Interface describing an interpreter which does not produce anything.
    /// </summary>
    public interface IInterpreter
    {

        /// <summary>
        /// Interpret the <paramref name="inputString"/>.
        /// </summary>
        /// <param name="inputString">The input string to interpret.</param>
        void Interpret(string inputString);
    }
}