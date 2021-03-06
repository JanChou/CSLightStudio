﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{


    public class RegHelper_DeleAction : RegHelper_Type, ICLS_Type_Dele
    {

        public RegHelper_DeleAction(Type type, string setkeyword)
            : base(type, setkeyword)
        {

        }


        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            returntype = null;

            if (left is DeleEvent)
            {
                DeleEvent info = left as DeleEvent;
                Delegate calldele = null;
                if (right.value is DeleFunction) calldele = CreateDelegate(env.environment, right.value as DeleFunction);
                else if (right.value is DeleLambda) calldele = CreateDelegate(env.environment, right.value as DeleLambda);
                else if (right.value is Delegate) calldele = right.value as Delegate;

                if (code == '+')
                {
                    info._event.AddEventHandler(info.source, calldele);
                    return null;
                }
                else if (code == '-')
                {
                    info._event.AddEventHandler(info.source, calldele);
                    return null;
                }

            }
            else if(left is Delegate)
            {
                Delegate info = left as Delegate;
                Delegate calldele = null;
                if (right.value is DeleFunction) calldele = CreateDelegate(env.environment, right.value as DeleFunction);
                else if (right.value is DeleLambda) calldele = CreateDelegate(env.environment, right.value as DeleLambda);
                else if (right.value is Delegate) calldele = right.value as Delegate;
                if (code == '+')
                {
                    Delegate.Combine(info, calldele);
                    return null;
                }
                else if (code == '-')
                {
                    Delegate.Remove(info, calldele);
                }
            }
            return new NotSupportedException();
        }



        public Delegate CreateDelegate(ICLS_Environment env, DeleFunction delefunc)
        {
            CLS_Content content = new CLS_Content(env);
            DeleFunction _func = delefunc;
            Action dele = () =>
            {

                content.DepthAdd();
                content.CallThis = _func.callthis;
                content.CallType = _func.calltype;
                content.function = _func.function;
                var func = _func.calltype.functions[_func.function];

                //content.DefineAndSet(function._paramnames[0], function._paramtypes[0].type, param0);

                func.expr_runtime.ComputeValue(content);
                content.DepthRemove();
            };
            Delegate d = dele as Delegate;
            if ((Type)this.type != typeof(Action))
            {
                return Delegate.CreateDelegate(this.type, d.Target, d.Method);
            }
            else
            {
                return dele;
            }
        }


        public Delegate CreateDelegate(ICLS_Environment env, DeleLambda lambda)
        {
            CLS_Content content = lambda.content.Clone();
            var pnames = lambda.paramNames;
            var expr = lambda.expr_func;
            Action dele = () =>
            {
                content.DepthAdd();


                //content.DefineAndSet(pnames[0], typeof(T), param0);

                expr.ComputeValue(content);

                content.DepthRemove();
            };
            Delegate d = dele as Delegate;
            if ((Type)this.type != typeof(Action))
            {
                return Delegate.CreateDelegate(this.type, d.Target, d.Method);
            }
            else
            {
                return dele;
            }
        }
    }
}
