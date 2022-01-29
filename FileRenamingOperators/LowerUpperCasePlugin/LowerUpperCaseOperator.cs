using BatchRenamingCore;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileRenamingOperators.LowerUpperCasePlugin
{
    public class LowerUpperCaseOperator : FileRenamingOperator
    {
        public override string MagicWord => "LowerUpperCase";

        public override string Description => "To Lower/ Upper/... Case";

        protected bool _lowerCase= true;
        public bool IsLowerCase
        {
            get => _lowerCase;
            set
            {
                _lowerCase = value;
                NotifyPropertyChanged("IsLowerCase");
            }
        }

        protected bool _upperCase = false;
        public bool IsUpperCase
        {
            get => _upperCase;
            set
            {
                _upperCase = value;
                NotifyPropertyChanged("IsUpperCase");
            }
        }
        protected bool _titleCase = false;
        public bool IsTitleCase
        {
            get => _titleCase;
            set
            {
                _titleCase = value;
                NotifyPropertyChanged("IsTitleCase");
            }
        }
        protected bool _camelCase = false;
        public bool IsCamelCase
        {
            get => _camelCase;
            set
            {
                _camelCase = value;
                NotifyPropertyChanged("IsCamelCase");
            }
        }

        public override FileRenamingOperator Clone()
        {
            return new LowerUpperCaseOperator();
        }
        private string ToTitleCase(string textToChange)
        {
            System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder();

            foreach (char c in textToChange)
            {
                // Replace anything, but letters and digits, with space
                if (!Char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            string result = resultBuilder.ToString();

            // Make result string all lowercase, because ToTitleCase does not change all uppercase correctly
            result = result.ToLower();

            // Creates a TextInfo based on the "en-US" culture.
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            return myTI.ToTitleCase(result).Replace(" ", String.Empty);
        }

        public override void Rename(ICollection<FileNameBuilder> builders)
        {
            if (_lowerCase)
            {
                foreach (var builder in builders)
                {
                    string name = builder.Name;
                    builder.Name = name.ToLower();
                }
                return;
            }
            if (_upperCase)
            {
                foreach (var builder in builders)
                {
                    string name = builder.Name;
                    builder.Name = name.ToUpper();
                }
                return;
            }
            if (_titleCase)
            {
                foreach (var builder in builders)
                {
                    string name = builder.Name;
                    builder.Name = ToTitleCase(name);
                }
                return;
            }
            if (_camelCase)
            {
                foreach (var builder in builders)
                {
                    string name = builder.Name;
                    if (name.Length == 0) continue;
                    name = ToTitleCase(name); 
                    char[] a = name.ToCharArray();
                    a[0] = char.ToLower(a[0]);
                    builder.Name = new string(a);
                }
                return;
            }
        }
    }
}
