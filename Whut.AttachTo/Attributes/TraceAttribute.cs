namespace Whut.AttachTo.Attributes
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;

    using PostSharp.Aspects;

    /// <summary>
    ///     The trace attribute class.
    /// </summary>
    [Serializable]
    public class TraceAttribute : OnMethodBoundaryAspect
    {
        #region Fields

        /// <summary>
        ///     The method name.
        /// </summary>
        private string methodName = "UnknownMethodName";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Method invoked at build time to initialize the instance fields of the current aspect. This method is invoked
        ///     before any other build-time method.
        /// </summary>
        /// <param name="method">
        /// Method to which the current aspect is applied
        /// </param>
        /// <param name="aspectInfo">
        /// Reserved for future usage.
        /// </param>
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            base.CompileTimeInitialize(method, aspectInfo);

            if (method.DeclaringType != null)
            {
                this.methodName = string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
            }
        }

        /// <summary>
        /// Method executed <b>before</b> the body of methods to which this aspect is applied.
        /// </summary>
        /// <param name="args">
        /// Event arguments specifying which method
        ///     is being executed, which are its arguments, and how should the execution continue
        ///     after the execution of
        ///     <see cref="M:PostSharp.Aspects.IOnMethodBoundaryAspect.OnEntry(PostSharp.Aspects.MethodExecutionArgs)"/>.
        /// </param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            base.OnEntry(args);

            Trace.TraceInformation("{0}: Enter", this.methodName);
            Trace.Indent();
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        ///     in case that the method resulted with an exception.
        /// </summary>
        /// <param name="args">
        /// Event arguments specifying which method is being executed and which are its arguments.
        /// </param>
        public override void OnException(MethodExecutionArgs args)
        {
            base.OnException(args);

            var stringBuilder = new StringBuilder(1024);

            stringBuilder.AppendFormat("{0}(", this.methodName);
            var instance = args.Instance;
            if (instance != null)
            {
                stringBuilder.AppendFormat("this={0}", instance);
                if (args.Arguments.Count > 0)
                {
                    stringBuilder.Append("; ");
                }
            }

            for (var i = 0; i < args.Arguments.Count; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(args.Arguments.GetArgument(i) ?? "null");
            }

            var guid = Guid.NewGuid();

            stringBuilder.AppendFormat(
                "): Exception {0} - {1}: {2}", 
                guid, 
                args.Exception.GetType().Name, 
                args.Exception.Message);

            Trace.Unindent();
            Trace.TraceError(stringBuilder.ToString());

            throw new ApplicationException(
                string.Format(
                    "An internal exception has occurred. Use the id {0} for further reference to this issue.", 
                    guid));
        }

        /// <summary>
        /// Method executed <b>after</b> the body of methods to which this aspect is applied,
        ///     but only when the method successfully returns (i.e. when no exception flies out
        ///     the method.).
        /// </summary>
        /// <param name="args">
        /// Event arguments specifying which method is being executed and which are its arguments.
        /// </param>
        public override void OnSuccess(MethodExecutionArgs args)
        {
            base.OnSuccess(args);

            Trace.Unindent();
            Trace.TraceInformation("{0}: Success", this.methodName);
        }

        #endregion
    }
}