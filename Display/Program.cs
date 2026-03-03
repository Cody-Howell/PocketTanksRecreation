using Display;
using Silk.NET.Maths;
using Silk.NET.Windowing;

var options = WindowOptions.Default;
options.Size = new Vector2D<int>(1000, 1000);
options.Title = "Test Window";

SilkClass display = new(options);
display.Run();
