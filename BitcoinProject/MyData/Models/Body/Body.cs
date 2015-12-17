using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.ComponentModel.Design;
using Util;

namespace Models.Body
{
	public abstract class Body : SerializableToBinary
	{
		// TODO: Include constant for message name in each body implementation

		public static Body getBody(string command, Stream input){

			// Grab all types in the Body namespace
			List<Type> types = Assembly.GetExecutingAssembly().GetTypes().ToList().Where(t => t.Namespace == "Models.Body").ToList();
			// From that grab the specific type that implements
			Type target = types.Find (t => t.GetCustomAttribute<BodyCommand>() != null && t.GetCustomAttribute<BodyCommand> ().Value.Equals (command));
			if(target == null){
				throw new UnsupportedCommandException ();
			}

			Body body = (Body) Activator.CreateInstance (target);
			body.Inflate (input);

			return body;
		}

		public abstract uint GetPayloadSize ();

		public abstract void Inflate(Stream input);

		#region SerializableToBinary implementation

		public abstract byte[] Serialize ();

		#endregion
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class BodyCommand : Attribute {

		public BodyCommand(string Value){
			this.Value = Value;
		}

		public string Value { get; set; }

	}
}

