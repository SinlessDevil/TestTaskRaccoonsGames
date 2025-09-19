using System.Collections.Generic;
using Code.StaticData;
using Code.StaticData.CubeData;
using Code.StaticData.Levels;
using Code.Window;

namespace Code.Services.StaticData
{
    public interface IStaticDataService
    {
        GameStaticData GameConfig { get; }
        BalanceStaticData Balance { get; }
        CubeStaticData CubeStaticData { get; }
        CubeColorStaticData CubeColorStaticData { get; }
        CubeSpawnStaticData CubeSpawnStaticData { get; }
        List<ChapterStaticData> Chapters { get; }
        void LoadData();
        WindowConfig ForWindow(WindowTypeId windowTypeId);
        LevelStaticData ForLevel(int chapterId, int levelId);
        ChapterStaticData ForChapter(int chapterId);
    }
}