﻿using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool TryConnect()
        {
            try
            {
                // This is used to create a connection using the given configuraion
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                Thread.Sleep(2000);

                // This is used to create a connection using the given configuraion
                _connection = _connectionFactory.CreateConnection();
            }
            return IsConnected;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connection");
            }

            // Create a chenner inside a Connection
            // We perform queue operation over this channel
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            try
            {
                _disposed = true;
                _connection.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
