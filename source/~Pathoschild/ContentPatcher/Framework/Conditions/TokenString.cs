using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ContentPatcher.Framework.Tokens;
using Pathoschild.Stardew.Common.Utilities;

namespace ContentPatcher.Framework.Conditions
{
    /// <summary>A string value which can contain condition tokens.</summary>
    internal class TokenString
    {
        /*********
        ** Fields
        *********/
        /// <summary>The regex pattern matching a string token.</summary>
        private static readonly Regex TokenPattern = new Regex(@"{{([ \w\.\-]+)}}", RegexOptions.Compiled);

        /// <summary>The underlying value for <see cref="Value"/>.</summary>
        private string ValueImpl;

        /// <summary>The underlying value for <see cref="IsReady"/>.</summary>
        private bool IsReadyImpl;


        /*********
        ** Accessors
        *********/
        /// <summary>The raw string without token substitution.</summary>
        public string Raw { get; }

        /// <summary>The tokens used in the string.</summary>
        public HashSet<TokenName> Tokens { get; } = new HashSet<TokenName>();

        /// <summary>The unrecognised tokens in the string.</summary>
        public InvariantHashSet InvalidTokens { get; } = new InvariantHashSet();

        /// <summary>Whether the string contains any tokens (including invalid tokens).</summary>
        public bool HasAnyTokens => this.Tokens.Count > 0 || this.InvalidTokens.Count > 0;

        /// <summary>Whether the token string value may change depending on the context.</summary>
        public bool IsMutable { get; }

        /// <summary>Whether the token string consists of a single token with no surrounding text.</summary>
        public bool IsSingleTokenOnly { get; }

        /// <summary>The string with tokens substituted for the last context update.</summary>
        public string Value => this.ValueImpl;

        /// <summary>Whether all tokens in the value have been replaced.</summary>
        public bool IsReady => this.IsReadyImpl;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="raw">The raw string before token substitution.</param>
        /// <param name="tokenContext">The available token context.</param>
        public TokenString(string raw, IContext tokenContext)
        {
            // set raw value
            this.Raw = raw?.Trim();
            if (string.IsNullOrWhiteSpace(this.Raw))
            {
                this.ValueImpl = this.Raw;
                this.IsReadyImpl = true;
                return;
            }

            // extract tokens
            int tokensFound = 0;
            foreach (Match match in TokenString.TokenPattern.Matches(this.Raw))
            {
                tokensFound++;
                string rawToken = match.Groups[1].Value.Trim();
                if (TokenName.TryParse(rawToken, out TokenName name))
                {
                    if (tokenContext.Contains(name, enforceContext: false))
                        this.Tokens.Add(name);
                    else
                        this.InvalidTokens.Add(rawToken);
                }
                else
                    this.InvalidTokens.Add(rawToken);
            }

            // set metadata
            this.IsMutable = this.Tokens.Any();
            if (!this.IsMutable)
            {
                this.ValueImpl = this.Raw;
                this.IsReadyImpl = !this.InvalidTokens.Any();
            }
            this.IsSingleTokenOnly = tokensFound == 1 && TokenString.TokenPattern.Replace(this.Raw, "", 1) == "";
        }

        /// <summary>Update the <see cref="Value"/> with the given tokens.</summary>
        /// <param name="context">Provides access to contextual tokens.</param>
        /// <returns>Returns whether the value changed.</returns>
        public bool UpdateContext(IContext context)
        {
            if (!this.IsMutable)
                return false;

            string prevValue = this.Value;
            this.GetApplied(context, out this.ValueImpl, out this.IsReadyImpl);
            return this.Value != prevValue;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get a new string with tokens substituted.</summary>
        /// <param name="context">Provides access to contextual tokens.</param>
        /// <param name="result">The input string with tokens substituted.</param>
        /// <param name="isReady">Whether all tokens in the <paramref name="result"/> have been replaced.</param>
        private void GetApplied(IContext context, out string result, out bool isReady)
        {
            bool allReplaced = true;
            result = TokenString.TokenPattern.Replace(this.Raw, match =>
            {
                TokenName name = TokenName.Parse(match.Groups[1].Value);
                IToken token = context.GetToken(name, enforceContext: true);
                if (token != null)
                    return token.GetValues(name).FirstOrDefault();
                else
                {
                    allReplaced = false;
                    return match.Value;
                }
            });
            isReady = allReplaced;
        }
    }
}
