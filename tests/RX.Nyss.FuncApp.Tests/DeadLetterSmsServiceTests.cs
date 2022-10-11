using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RX.Nyss.FuncApp.Configuration;
using RX.Nyss.FuncApp.Services;
using Xunit;

namespace RX.Nyss.FuncApp.Tests
{
    public class DeadLetterSmsServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly IConfig _config;
        private readonly ILogger<DeadLetterSmsService> _logger;
        private readonly DeadLetterSmsService _deadLetterSmsService;
        private readonly ServiceBusClient _serviceBusClientMock;
        private ServiceBusSender serviceBusSenderMock;
        private ServiceBusReceiver serviceBusReceiverMock;


        public DeadLetterSmsServiceTests()
        {
            _configuration = Substitute.For<IConfiguration>();
            _config = Substitute.For<IConfig>();
            _config.MailConfig = new NyssFuncAppConfig.MailConfigOptions { EnableFeedbackSms = true };
            _logger = Substitute.For<ILogger<DeadLetterSmsService>>();
            _serviceBusClientMock = Substitute.For<ServiceBusClient>();
            serviceBusSenderMock = Substitute.For<ServiceBusSender>();
            _serviceBusClientMock.CreateSender("").Returns(serviceBusSenderMock);
            serviceBusReceiverMock = Substitute.For<ServiceBusReceiver>();

            serviceBusReceiverMock.ReceiveMessageAsync(new TimeSpan?(), CancellationToken.None)
                .Returns(Task.FromResult(Substitute.For<ServiceBusReceivedMessage>()));
            _serviceBusClientMock.CreateReceiver("").Returns(serviceBusReceiverMock);

            _deadLetterSmsService = new DeadLetterSmsService(
                _config,
                _configuration,
                _logger,
                _serviceBusClientMock);
        }

        [Fact]
        public async Task ResubmitDeadLetterMessages_DisabledResendingFeedbackMessages_NoMessagesSent()
        {
            _config.MailConfig = new NyssFuncAppConfig.MailConfigOptions { EnableFeedbackSms = false };

            // Act
            await _deadLetterSmsService.ResubmitDeadLetterMessages();

            // Assert
            serviceBusSenderMock.DidNotReceive();
        }

        [Fact]
        public async Task ResubmitDeadLetterMessages_EnabledResendingFeedbackMessages_MessagesSent()
        {
            _config.MailConfig = new NyssFuncAppConfig.MailConfigOptions { EnableFeedbackSms = true };

            // Act
            await _deadLetterSmsService.ResubmitDeadLetterMessages();

            // Assert
            serviceBusSenderMock.Received();
            serviceBusReceiverMock.Received();
        }

    }
}
