using System.ComponentModel;

namespace SpartaDungeonPractice
{
    internal class Program
    {
        private static PlayerStat playerStat;
        private static ItemData itemData;
        // 플레이어가 아이템을 구매하면, ItemInDatabase -> playerInventory -> playuerEquippedItems로 넘어가는 구조
        static List<ItemData> itemsInDatabase = new List<ItemData>();
        static List<ItemData> playerEquippedItems = new List<ItemData>();

        static void Main(string[] args)
        {
            InitItemDatabase();
            PlayerDataSet();
            MainGameScene();

        }
        static void InitItemDatabase()
        {
            itemsInDatabase.Add(new ItemData(0, "낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검입니다."));
            itemsInDatabase.Add(new ItemData(1, "천 갑옷", 0, 2, "질긴 천을 덧대어 제작한 낡은 갑옷입니다."));
            itemsInDatabase.Add(new ItemData(2, "헤라클레스의 곤봉", 5, 0, "이 곤봉은 12가지 과업을 대비해서 갖고 다녀야합니다."));
            itemsInDatabase.Add(new ItemData(3, "포세이돈의 삼지창", 10, 0, "이 삼지창을 쥐면 바다를 다스릴 수 있다는 소문 때문에 선원들이 탐내는 무기입니다."));
            itemsInDatabase.Add(new ItemData(4, "헤르메스 트리스메기투스의 지팡이", 30, 0, "미지의 세계, 아틀란티스로 갈 수 있는 열쇠입니다."));
        }
        static void PlayerDataSet()
        {
            Console.Title = "닉네임을 설정하세요!";
            Console.WriteLine("게임에 사용하실 닉네임을 입력해주세요!");
            Console.Write(">>");
            // 게임 닉네임 설정 및 데이터 설정
            string _inputName = Console.ReadLine();

            if (_inputName != null)
            {
                Console.Clear();
                playerStat = new PlayerStat($"{_inputName}", "전사", 1, 10, 5, 100, 1500);
            }
            else
            {
                Console.WriteLine("이름을 입력해주세요!");
            }
        }

        static void MainGameScene()
        {
            Console.Title = "스파르타 던전";
            SetConsoleColor(ConsoleColor.Red);
            Console.WriteLine("Sparta Dungeon Game!");
            Console.ResetColor();
            SetConsoleColor(ConsoleColor.Cyan);
            Console.Write($"{playerStat.Name} ");
            Console.ResetColor();
            Console.WriteLine("님, 스파르타 마을에 오신것을 환영합니다!\n");
            Console.WriteLine("이곳에서 던전으로 돌아가기 전 활동을 할 수 있습니다.\n");
            Console.WriteLine("0. 게임 종료");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine(" ");
            int _input = CheckValidAction(0, 2);

            switch (_input)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    DisplayPlayerState();
                    break;
                case 2:
                    DisplayPlayerInventory();
                    break;
            }
        }

        static void DisplayPlayerInventory()
        {
            Console.Clear();
            Console.Title = "인벤토리";
            Console.WriteLine("[인벤토리]");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");

            string ItemEquipped;
            for (int i = 0; i < itemsInDatabase.Count; i++)
            {
                ItemData item = itemsInDatabase[i];
                
                ItemEquipped = item.IsItemEquipped ? "[E] " : "";
                if (item.IsItemEquipped)
                {
                    SetConsoleColor(ConsoleColor.Yellow);
                }
                Console.Write($"{ItemEquipped}");
                Console.ResetColor();
                Console.Write($"{item.ItemName}");
                DisplayAtkOrDef(item); // DisplayAtkOrDef 메서드에 아이템 객체를 전달
                Console.WriteLine($" {item.ItemComm} ");
            }
            Console.WriteLine(" ");
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");

            int _input = CheckValidAction(0, 1);

            switch (_input)
            {
                case 0:
                    Console.Clear();
                    MainGameScene();
                    break;
                case 1:
                    Console.Clear();
                    ManagementPlayerInventory();
                    break;
            }
        }

        static void ManagementPlayerInventory()
        {
            Console.Clear();
            // 콘솔창 타이틀 변경
            Console.Title = "인벤토리 - 장착관리";
            Console.WriteLine("[인벤토리 - 장착관리]");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");
            string ItemEquipped;
            for (int i = 0; i < itemsInDatabase.Count; i++)
            {
                ItemData item = itemsInDatabase[i];
                ItemEquipped = item.IsItemEquipped ? "[E] " : "";
                if (item.IsItemEquipped)
                {
                    SetConsoleColor(ConsoleColor.Yellow);
                }
                Console.Write($"{ItemEquipped}");
                Console.ResetColor();
                Console.Write($"{item.ItemName}");
                DisplayAtkOrDef(item); // DisplayAtkOrDef 메서드에 아이템 객체를 전달
                
                Console.WriteLine($" {item.ItemComm} ");
            }
            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기");
            int _input = CheckValidAction(0, itemsInDatabase.Count);
            
            if(_input == 0)
            {
                Console.Clear();
                DisplayPlayerInventory();
            }
            else if (_input > 0 && _input <= itemsInDatabase.Count)
            {
                // 아이템 인덱스는 0부터 시작!
                ItemData selectedItem = itemsInDatabase[_input -1];
                ToggleEquip(selectedItem);
                ManagementPlayerInventory();
            }
        }

        static void ToggleEquip(ItemData item)
        {
            bool isAtkItemEqupped = IsAtkItemEquipped();

            bool isDefItemEquipped = IsDefItemEquipped();

            // 아이템의 공격력이 부여되고, 아이템이 장착되고, 아이템 착용 상태라면
            if (item.ItemAtk > 0 && isAtkItemEqupped && !item.IsItemEquipped)
            {
                return;
            }

            // 아이템의 방어력이 부여되고, 아이템이 장착되고, 아이템 착용 상태라면
            if (item.ItemDef > 0 && isDefItemEquipped && !item.IsItemEquipped)
            {
                return;
            }

            item.IsItemEquipped = !item.IsItemEquipped;

            if (item.IsItemEquipped)
            {
                playerEquippedItems.Add(item);
            }
            else
            {
                playerEquippedItems.Remove(item);
            }

            UpdatePlayerStats();
        }

        static void UpdatePlayerStats()
        {
            int totalAtk = 0;
            int totalDef = 0;

            foreach(ItemData item in playerEquippedItems)
            {
                totalAtk += item.ItemAtk;
                totalDef += item.ItemDef;
            }

            playerStat.AtkValue = playerStat.BaseAtkValue + totalAtk;
            playerStat.DefValue = playerStat.BaseDefValue + totalDef;
        }
        static void DisplayAtkOrDef(ItemData item)
        {
            if (item.ItemAtk > 0 && item.ItemDef == 0)
            {
                Console.Write($"| 공격력 + {item.ItemAtk} |");
            }
            else if (item.ItemAtk == 0 && item.ItemDef > 0)
            {
                Console.Write($"| 방어력 + {item.ItemDef} |");
            }
            
        }
        static bool IsAtkItemEquipped()
        {
            foreach (ItemData item in playerEquippedItems)
            {
                if (item.ItemAtk > 0)
                {
                    return true;
                }
            }
            return false;
        }

        static bool IsDefItemEquipped()
        {
            foreach(ItemData item in playerEquippedItems)
            {
                if(item.ItemDef > 0)
                {
                    return true;
                }
            }
            return false;
        }



        static void DisplayPlayerState()
        {
            Console.Clear();
            Console.WriteLine($"상태보기");
            Console.WriteLine($"캐릭터의 정보가 표시됩니다.");
            Console.WriteLine($"Lv. {playerStat.Level}");
            Console.WriteLine($"{playerStat.Name} ( {playerStat.PlayerClass} )");
            Console.WriteLine($"공격력 : {playerStat.AtkValue}");
            Console.WriteLine($"방어력 : {playerStat.DefValue}");
            Console.WriteLine($"체 력 : {playerStat.HpValue}"); ;
            Console.WriteLine($"Gold : {playerStat.Gold} G");
            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기");
            int _input = CheckValidAction(0, 0);

            switch (_input)
            {
                case 0:
                    Console.Clear();
                    MainGameScene();
                    break;
            }
        }



        static int CheckValidAction(int _min, int _max)
        {
            while (true)
            {
                Console.WriteLine(" ");
                Console.WriteLine(" 원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                string _input = Console.ReadLine();

                bool _parseSuccess = int.TryParse(_input, out var _ret);
                if (_parseSuccess)
                {
                    if (_ret >= _min && _ret <= _max)
                        return _ret;
                }
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        static void SetConsoleColor(ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
        }
    }

    public class PlayerStat
    {
        public string Name { get; set; }
        public string PlayerClass { get; }
        public int Level { get; }
        public int AtkValue { get; set; }
        public int DefValue { get; set; }
        public int HpValue { get; }
        public int Gold { get; }
        public int BaseAtkValue { get; set; }
        public int BaseDefValue { get; set; }

        public PlayerStat(string _name, string _playerClass, int _level, int _atkValue, int _defValue, int _hpValue, int _gold)
        {
            Name = _name;
            PlayerClass = _playerClass;
            Level = _level;
            AtkValue = _atkValue;
            DefValue = _defValue;
            HpValue = _hpValue;
            Gold = _gold;

            BaseAtkValue = _atkValue;
            BaseDefValue = _defValue;
        }

    }

    public class ItemData
    {
        public int ItemId { get; set; }
        public bool IsItemEquipped { get; set; }
        public string ItemName { get; set; }
        public int ItemAtk { get; set; }
        public int ItemDef { get; set; }
        public string ItemComm { get; set; }

        public ItemData(int _itemId, string _itemName, int _itemAtk, int _itemDef, string _itemComm)
        {
            ItemId = _itemId;
            ItemName = _itemName;
            ItemAtk = _itemAtk;
            ItemDef = _itemDef;
            ItemComm = _itemComm;
            IsItemEquipped = false;
        }
    }
}