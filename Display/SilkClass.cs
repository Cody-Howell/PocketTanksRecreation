using Silk.NET.Input;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using System.Drawing;
using ImGuiNET;
using System.Numerics;
using Logic;
using HowlDev.Simulation.Physics.Primitive2D;

namespace Display;

public class SilkClass : IDisposable {
    protected IWindow window;
    protected GL? _gl;
    protected uint _vao;
    protected uint _vbo;
    protected uint _program;
    protected IInputContext? input;
    protected ImGuiController? controller;
    protected (int x, int y) windowSize;
    protected Game g;

    protected string VertexShader = """
    #version 330 core
    layout (location = 0) in vec3 aPos;
    layout (location = 1) in vec4 aColor;
    out vec4 vertexColor;
    void main() {
        gl_Position = vec4(aPos, 1.0);
        vertexColor = aColor;
    }
    """;

    protected string FragmentShader = """
    #version 330 core
    in vec4 vertexColor;
    out vec4 FragColor;
    void main() {
        FragColor = vertexColor;
    }
    """;

    public SilkClass(WindowOptions options) {
        window = Window.Create(options);
        windowSize = (options.Size.X, options.Size.Y);
        g = new();

        // Wire up event handlers
        window.Load += OnLoad;
        window.Render += OnRender;
    }

    protected virtual unsafe void OnLoad() {
        _gl = GL.GetApi(window);

        uint vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        _gl.ShaderSource(vertexShader, VertexShader);
        _gl.CompileShader(vertexShader);

        uint fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
        _gl.ShaderSource(fragmentShader, FragmentShader);
        _gl.CompileShader(fragmentShader);

        _program = _gl.CreateProgram();
        _gl.AttachShader(_program, vertexShader);
        _gl.AttachShader(_program, fragmentShader);
        _gl.LinkProgram(_program);

        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);

        input = window.CreateInput();
        controller = new ImGuiController(
            _gl,
            window,
            input
        );
        foreach (IMouse mouse in input.Mice) {
            mouse.MouseDown += OnMouseDown;
        }

        foreach (IKeyboard keyboard in input.Keyboards) {
            keyboard.KeyDown += OnKeyDown;
            keyboard.KeyUp += OnKeyUp;
        }

        _vao = _gl.GenVertexArray();
        _vbo = _gl.GenBuffer();
        _gl.BindVertexArray(_vao);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

        _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(3 * 7 * sizeof(float)), null, BufferUsageARB.DynamicDraw);

        _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), (void*)0);
        _gl.EnableVertexAttribArray(0);

        _gl.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), (void*)(3 * sizeof(float)));
        _gl.EnableVertexAttribArray(1);
    }

    protected virtual void OnMouseDown(IMouse mouse, MouseButton button) {
        g.Floor.CalculateExplosion(new Point2D(mouse.Position.X, windowSize.y - mouse.Position.Y), 50);
        Console.WriteLine($"Click at: {mouse.Position.X}, {mouse.Position.Y} with {button}");
    }

    protected virtual void OnKeyDown(IKeyboard keyboard, Key key, int keyCode) {
        if (key == Key.Escape)
            window.Close();

        Console.WriteLine($"Key Pressed: {key} (Native ID: {keyCode})");
    }

    protected virtual void OnKeyUp(IKeyboard keyboard, Key key, int keyCode) {
        Console.WriteLine($"Key Released: {key}");
    }

    protected virtual void OnRender(double deltaTime) {
        if (_gl is null || input is null || controller is null) return;

        var mousePos = input.Mice[0].Position;

        var io = ImGui.GetIO();

        float x = mousePos.X / window.Size.X * 2 - 1;
        float y = mousePos.Y / window.Size.Y * -2 + 1;

        controller.Update((float)deltaTime);

        _gl.ClearColor(Color.FromArgb(255, (int)(.45f * 255), (int)(.55f * 255), (int)(.60f * 255)));
        _gl.Clear((uint)ClearBufferMask.ColorBufferBit);

        var drawList = ImGui.GetBackgroundDrawList();
        var foreground = ImGui.GetForegroundDrawList();

        foreground.AddText(new Vector2(10, 10), 0xFFFFFFFF, $"Mouse: {Math.Round(x, 2)}, {Math.Round(y, 2)}");

        _gl.UseProgram(_program);

        // Draw the floor
        Point2D[] points = g.Floor.GetPoints();
        // foreground.AddText(new Vector2(10, 50), 0xFFFFFFFF, $"{string.Join(';', points)}");
        for (int i = 1; i < points.Length; i++) {
            drawList.AddTriangleFilled(
                new Vector2((float)points[i - 1].X, windowSize.y - (float)points[i - 1].Y),
                new Vector2((float)points[i - 1].X, windowSize.y),
                new Vector2((float)points[i].X, windowSize.y),
                0xFFFFFFFF
            );
            drawList.AddTriangleFilled(
                new Vector2((float)points[i - 1].X, windowSize.y - (float)points[i - 1].Y),
                new Vector2((float)points[i].X, windowSize.y),
                new Vector2((float)points[i].X, windowSize.y - (float)points[i].Y),
                0xFFFFFFFF
            );
        }

        drawList.AddCircleFilled(new Vector2(mousePos.X, mousePos.Y), 50.0f, 0xFF00FFFF);

        controller.Render();
    }

    public void Run() {
        window.Run();
    }

    public void Dispose() {
        window?.Dispose();
    }
}
