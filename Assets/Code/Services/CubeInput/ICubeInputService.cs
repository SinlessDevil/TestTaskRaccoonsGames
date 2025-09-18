namespace Code.Services.CubeInput
{
    public interface ICubeInputService
    {
        void SetupCube(Cube cube);
        void Enable();
        void Disable();
    }
}
