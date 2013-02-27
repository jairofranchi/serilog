using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog.Parsing;
using Serilog.Values;

namespace Serilog.Core
{
    class MessageTemplate
    {
        private readonly IEnumerable<MessageTemplateToken> _tokens;

        public MessageTemplate(IEnumerable<MessageTemplateToken> tokens)
        {
            _tokens = tokens;
        }

        public IEnumerable<LogEventProperty> ConstructPositionalProperties(object[] positionalValues)
        {
            if (positionalValues == null)
                yield break;

            var next = 0;
            foreach (var propertyToken in _tokens.OfType<LogEventPropertyToken>())
            {
                if (next < positionalValues.Length)
                {
                    var value = positionalValues[next];
                    yield return new LogEventProperty(
                        propertyToken.PropertyName,
                        LogEventPropertyValue.For(value, propertyToken.DestructuringHint));
                    next++;
                }
                else
                {
                    yield break;
                }
            }
        }

        public void Render(IReadOnlyDictionary<string, LogEventProperty> properties, TextWriter output)
        {
            foreach (var token in _tokens)
            {
                token.Render(properties, output);
            }
        }
    }
}