using FluentAssertions;
using HelloChatApp.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HelloChatApp.Tests.Server.Models
{
    public class MessageDetailsTest
    {
        [Fact]
        public void IsCommand_shoul_return_true_as_message_begins_with_command_prefix()
        {
            //Arrange
            var room = "A1";            
            var message = "/stock=EET_20";
            var userName = "Connor";
            var userHubId = "1234";

            //Act
            var messageDetails = new MessageDetails(room, message, userName, userHubId);

            //Assert
            messageDetails.IsCommand.Should().BeTrue();
        }

        [Fact]
        public void IsCommand_shoul_return_true_as_message_does_not_begin_with_command_prefix()
        {
            //Arrange
            var room = "A1";
            var message = "hello my friend, stay a while and listen";
            var userName = "Connor";
            var userHubId = "1234";

            //Act
            var messageDetails = new MessageDetails(room, message, userName, userHubId);

            //Assert
            messageDetails.IsCommand.Should().BeFalse();
        }
    }
}
