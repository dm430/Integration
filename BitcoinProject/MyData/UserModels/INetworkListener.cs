using System;

namespace UserModels
{
	public interface INetworkListener
	{
		void OnMessageReceived(InstantMessage msg);
		void OnRegistrationReceived(UserRegistration registration);
	}
}

