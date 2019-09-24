﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SyokumotsuRensa.CSV;

namespace SyokumotsuRensa
{
   public class Wave
    {
      public static int currentWave;
        int nextWave;

       public readonly int FianlWave = 3;

        bool isEndFlag;
        bool isClearFlag;
        private List<Enemy> eL1List;
        private Camp camp;
        private List<Player> players;
        private List<Wall> walls;
        private List<Unchi> unchis;
        private List<Glass> glasses;
        EnemyCSVParser parser;

        public Wave(Camp camp, List<Player> players, List<Wall> walls, List<Unchi> unchis, List<Glass> glasses, bool isClearFlag, bool isEndFlag)
        {
            currentWave = 0;

            this.camp = camp;
            this.players = players;
            this.walls = walls;
            this.unchis = unchis;
            this.glasses = glasses;
            this.isClearFlag = isClearFlag;
            this.isEndFlag = isEndFlag;
        }

        public void Initialize()
        {
            CSVReader csvReader = new CSVReader();
            csvReader.Read("spawn1.csv");

            eL1List = new List<Enemy>();

            parser = new EnemyCSVParser(camp, players, walls, unchis, glasses);
            var dataList = parser.Parse("spawn1.csv", "./");
            foreach (var data in dataList)
            {
                //eL1List.Add(data);
            }


            foreach (var el1 in eL1List)
            {
                el1.Initialize();
            }
        }

        public void Update()
        {
            Console.WriteLine(players.Count);
            nextWave = currentWave + 1;

            if (nextWave > FianlWave && isEndFlag)
            {
                //終了処理


                return;
            }

            if (Player.playerStock <= 0)
            {
                isEndFlag = true;
            }

            if (eL1List.Count == 0 && !isEndFlag)
            {
                isClearFlag = true;
            }

            if (isClearFlag && Input.GetKeyTrigger(Keys.Space))
            {
                GotoWave();
                isClearFlag = false;
            }

            foreach (var el1 in eL1List)
            {
                el1.Update();
            }


            for (int i = eL1List.Count - 1; i >= 0; i--)
            {
                if (eL1List[i].moveEndFlag)
                {
                    eL1List.RemoveAt(i);
                }
                else if (eL1List[i].stuffMAXFlag)
                {
                    eL1List.RemoveAt(i);
                }
            }

            for (int i = unchis.Count - 1; i >= 0; i--)
            {
                if (unchis[i].iswwwFlag)
                {
                    unchis.RemoveAt(i);
                }
            }

        }

        public void Draw(Renderer renderer)
        {
            foreach (var el1 in eL1List)
            {
                el1.Draw(renderer);
            }
           
            //ゲームオーバー
            if (isEndFlag)
            {
                renderer.DrawTexture("GameOver", new Vector2(350, 0));
            }
            if (isClearFlag)
            {
                renderer.DrawTexture("GameClear", new Vector2(350, 0));
            }
        }

        public int NowWave()
        {
            return currentWave;
        }

        public int NextWave()
        {
            return nextWave;
        }


        public void GotoWave()
        {
            currentWave = nextWave;

            players.Clear();
            foreach (var pl in players)
            {
                pl.Initialize();
            }

            Console.WriteLine(players.Count);
            
            CSVReader csvReader = new CSVReader();
            csvReader.Read("spawn"+ currentWave.ToString() +".csv");

            eL1List = new List<Enemy>();
            
            var dataList = parser.Parse("spawn" + currentWave.ToString() + ".csv", "./");
            foreach (var data in dataList)
            {
                eL1List.Add(data);
            }


            foreach (var el1 in eL1List)
            {
                el1.Initialize();
            }
        }
    }
}
