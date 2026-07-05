using ComputerysBitStream.Extras.Settings;

[assembly: DefaultBitStreamSettings(typeof(ComputerysBitStream.Tests.ITestSettings))]

namespace ComputerysBitStream.Tests;

[BitStreamSettings]
public interface ITestSettings : IGameExtrasSettings { }
