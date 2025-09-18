namespace Code.Services.CubeInput
{
    public interface ICubeInputService
    {
        void SetupCube(Cube cube);
        void SetBoundaries(float leftPosition, float rightPosition);
        void SetSmoothSpeed(float smoothSpeed);
        void Enable();
        void Disable();
    }
}
