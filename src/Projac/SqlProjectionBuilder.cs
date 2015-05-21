﻿using System;
using System.Collections.Generic;
using System.Linq;
using Paramol;

namespace Projac
{
    /// <summary>
    ///     Represents a fluent syntax to build up a set of <see cref="SqlProjectionHandler" />.
    /// </summary>
    public class SqlProjectionBuilder
    {
        private readonly SqlProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlProjectionBuilder" /> class.
        /// </summary>
        public SqlProjectionBuilder() :
            this(new SqlProjectionHandler[0])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlProjectionBuilder" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="handlers"/> is <c>null</c>.</exception>
        public SqlProjectionBuilder(SqlProjectionHandler[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the single non query command returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The single command returning handler.</param>
        /// <returns>A <see cref="SqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public SqlProjectionBuilder When<TMessage>(Func<TMessage, SqlNonQueryCommand> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new SqlProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new SqlProjectionHandler
                            (
                            typeof (TMessage),
                            message => new[] {handler((TMessage) message)}
                            )
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the non query command array returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The command array returning handler.</param>
        /// <returns>A <see cref="SqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public SqlProjectionBuilder When<TMessage>(Func<TMessage, SqlNonQueryCommand[]> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new SqlProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new SqlProjectionHandler
                            (
                            typeof (TMessage),
                            message => handler((TMessage) message)
                            )
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the non query statement enumeration returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The non query command enumeration returning handler.</param>
        /// <returns>A <see cref="SqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public SqlProjectionBuilder When<TMessage>(Func<TMessage, IEnumerable<SqlNonQueryCommand>> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new SqlProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new SqlProjectionHandler
                            (
                            typeof (TMessage),
                            message => handler((TMessage) message)
                            )
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Builds a set of the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="SqlProjectionHandler" /> array.</returns>
        public SqlProjectionHandler[] Build()
        {
            return _handlers;
        }
    }
}