using System;
using System.Collections.Generic;
using Controller;

namespace Stage
{
    public class Stage
    {
        public enum StageType
        {
            /// <summary>
            /// 一定数の敵を倒す
            /// </summary>
            KillCount,

            /// <summary>
            /// 時間内に倒した敵の数や自機残体力などでスコア換算
            /// </summary>
            TimeKill,

            /// <summary>
            /// 用意された敵をすべて倒しきる
            /// </summary>
            KillAll
        }

        private StageType _stageType;

        private int _enemyCount;

        public int EnemyCount => _enemyCount;

        private int _limitTime;

        public int LimitTime => _limitTime;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Stage(int stageNumber)
        {
            List<string[]> stageData = DataController.Instance.StageData;
            Enum.TryParse(stageData[stageNumber][0], out _stageType);
            _enemyCount = int.Parse(stageData[stageNumber][1]);
            _limitTime = int.Parse(stageData[stageNumber][2]);
        }
    }
}