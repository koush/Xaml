using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyObjectCodeGenerator
{
    public enum Modifiers
    {
        @private,
        @public,
        @protected,
        @internal
    }

    public enum DependencyPropertyFlags
    {
        AffectsNothing,
        AffectsVisual,
        AffectsMeasure,
    }

    public class DependencyProperty
    {
        public string Name;
        public string Type;
        public Modifiers Modifier = Modifiers.@public;
        public bool Storage = true;
        public string DefaultValue = "null";
        public bool Accessor = true;
        public bool Attached = false;
        public DependencyPropertyFlags Flags = DependencyPropertyFlags.AffectsMeasure;
        public string Handler = null;

        internal void Write(StringBuilder builder)
        {
            string handler = Handler;
            if (handler != null)
                handler = string.Format("new DependencyPropertyChangedHandler<{0}>({1})", Type, handler);
            else
                handler = "null";
            builder.AppendFormat("\t\t{0} static readonly DependencyProperty {1}Property = DependencyProperty.Register<{2}>(\"{1}\", {5}, {3}, DependencyPropertyFlags.{4}, {6});", Modifier, Name, Type, Attached.ToString().ToLower(), Flags, DefaultValue, handler);
            builder.AppendLine();
            if (Storage)
            {
                builder.AppendFormat("\t\tDependencyPropertyStorage<{1}> my{0};", Name, Type);
                builder.AppendLine();
            }
            if (Accessor)
            {
                builder.AppendFormat("\t\t{0} {1} {2}", Modifier, Type, Name);
                builder.AppendLine();
                builder.Append("\t\t{");
                builder.AppendLine();

                builder.Append("\t\t\tget");
                builder.AppendLine();
                builder.Append("\t\t\t{");
                builder.AppendLine();
                if (Storage)
                {
                    builder.AppendFormat("\t\t\t\treturn my{0};", Name);
                    builder.AppendLine();
                }
                else
                {
                    builder.AppendFormat("\t\t\t\treturn GetValue<{0}>({1}Property);", Type, Name);
                    builder.AppendLine();
                }
                builder.Append("\t\t\t}");
                builder.AppendLine();

                builder.Append("\t\t\tset");
                builder.AppendLine();
                builder.Append("\t\t\t{");
                builder.AppendLine();
                if (Storage)
                {
                    builder.AppendFormat("\t\t\t\tmy{0}.Value = value;", Name);
                    builder.AppendLine();
                }
                else
                {
                    builder.AppendFormat("\t\t\t\tSetValue<{0}>({1}Property, value);", Type, Name);
                    builder.AppendLine();
                }
                builder.Append("\t\t\t}");
                builder.AppendLine();

                builder.Append("\t\t}");
                builder.AppendLine();
            }
        }

        internal void WriteStorage(StringBuilder builder)
        {
            builder.AppendFormat("\t\t\tmy{0} = GetStorage<{1}>({0}Property);", Name, Type);
            builder.AppendLine();
        }
    }
}
