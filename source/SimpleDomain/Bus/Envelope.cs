﻿//-------------------------------------------------------------------------------
// <copyright file="Envelope.cs" company="frokonet.ch">
//   Copyright (c) 2015
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace SimpleDomain.Bus
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// The message envelope
    /// </summary>
    public class Envelope
    {
        /// <summary>
        /// Creates a new instance of <see cref="Envelope"/>
        /// </summary>
        /// <param name="headers">The headers</param>
        /// <param name="body">The message body</param>
        [JsonConstructor]
        public Envelope(
            Dictionary<string, object> headers,
            IMessage body)
        {
            this.Headers = headers;
            this.Body = body;
        }

        /// <summary>
        /// Gets the headers
        /// </summary>
        public Dictionary<string, object> Headers { get; }

        /// <summary>
        /// Gets the message body
        /// </summary>
        public IMessage Body { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="Envelope"/>
        /// </summary>
        /// <param name="sender">The sending endpoint address</param>
        /// <param name="recipient">The receiving endpoint address</param>
        /// <param name="body">The message body</param>
        /// <returns>A new instance of <see cref="Envelope"/></returns>
        public static Envelope Create(EndpointAddress sender, EndpointAddress recipient, IMessage body)
        {
            var mesageId = Guid.NewGuid();
            var headers = new Dictionary<string, object>
            {
                { HeaderKeys.Sender, sender },
                { HeaderKeys.Recipient, recipient },
                { HeaderKeys.TimeSent, DateTime.UtcNow },
                { HeaderKeys.MessageId, mesageId },
                { HeaderKeys.CorrelationId, mesageId }
            };

            return new Envelope(headers, body);
        }
        
        /// <summary>
        /// Adds a single header to the header collection
        /// </summary>
        /// <param name="key">The header key</param>
        /// <param name="value">The header value</param>
        /// <returns>The envelope itself since this is a builder method</returns>
        public Envelope AddHeader(string key, object value)
        {
            if (this.Headers.ContainsKey(key))
            {
                return this;
            }

            this.Headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// Replaces a single header in the header collection
        /// </summary>
        /// <param name="key">The header key</param>
        /// <param name="value">The header value</param>
        /// <returns>The envelope itself since this is a builder method</returns>
        public Envelope ReplaceHeader(string key, object value)
        {
            if (this.Headers.ContainsKey(key))
            {
                this.Headers.Remove(key);
            }

            this.Headers.Add(key, value);
            return this;
        }
    }
}