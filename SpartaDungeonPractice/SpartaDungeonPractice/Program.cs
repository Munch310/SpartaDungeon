namespace SpartaDungeonPractice
{
    internal class Program
    {
        private static PlayerStat _playerStat;
        private static ItemData _itemData;
        static List<ItemData> _itemsInDatabase = new List<ItemData>();
        static List<ItemData> _playerEquippedItems = new List<ItemData>();

        static void Main(string[] args)
        {
            InitItemDatabase();
            PlayerDataSet();
            MainGameScene();
        }

        static void InitItemDatabase()
        {
            _itemsInDatabase.Add(new ItemData(0, "낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검입니다."));
            _itemsInDatabase.Add(new ItemData(1, "천 갑옷", 0, 2, "질긴 천을 덧대어 제작한 낡은 갑옷입니다."));
            _itemsInDatabase.Add(new ItemData(2, "헤라클레스의 곤봉", 5, 0, "이 곤봉은 12가지 과업을 대비해서 갖고 다녀야합니다."));
            _itemsInDatabase.Add(new ItemData(3, "포세이돈의 삼지창", 10, 0, "이 삼지창을 쥐면 바다를 다스릴 수 있다는 소문 때문에 선원들이 탐내는 무기입니다."));
            _itemsInDatabase.Add(new ItemData(4, "헤르메스 트리스메기투스의 지팡이", 30, 0, "미지의 세계, 아틀란티스로 갈 수 있는 열쇠입니다."));
        }

        static void PlayerDataSet()
        {
            Console.Title = "닉네임을 설정하세요!";
            Console.WriteLine("게임에 사용하실 닉네임을 입력해주세요!");
            Console.Write(">>");

            string inputName = Console.ReadLine();

            if (!string.IsNullOrEmpty(inputName))
            {
                Console.Clear();
                _playerStat = new PlayerStat($"{inputName}", "전사", 1, 10, 5, 100, 1500);
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
            Console.Write($"{_playerStat.Name} ");
            Console.ResetColor();
            Console.WriteLine("님, 스파르타 마을에 오신것을 환영합니다!\n");
            Console.WriteLine("이곳에서 던전으로 돌아가기 전 활동을 할 수 있습니다.\n");
            Console.WriteLine("0. 게임 종료");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine(" ");
            int input = CheckValidAction(0, 2);

            switch (input)
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

            string _itemEquipped;
            for (int i = 0; i < _itemsInDatabase.Count; i++)
            {
                ItemData _item = _itemsInDatabase[i];

                _itemEquipped = _item.IsItemEquipped ? "[E] " : "";
                if (_item.IsItemEquipped)
                {
                    SetConsoleColor(ConsoleColor.Yellow);
                }
                Console.Write($"{_itemEquipped}");
                Console.ResetColor();
                Console.Write($"{_item.ItemName}");
                DisplayAtkOrDef(_item);
                Console.WriteLine($" {_item.ItemComm} ");
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
            Console.Title = "인벤토리 - 장착관리";
            Console.WriteLine("[인벤토리 - 장착관리]");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");
            string _itemEquipped;
            for (int i = 0; i < _itemsInDatabase.Count; i++)
            {
                ItemData _item = _itemsInDatabase[i];
                _itemEquipped = _item.IsItemEquipped ? "[E] " : "";
                if (_item.IsItemEquipped)
                {
                    SetConsoleColor(ConsoleColor.Yellow);
                }
                Console.Write($"{_itemEquipped}");
                Console.ResetColor();
                Console.Write($"{_item.ItemId + 1}.{_item.ItemName}");
                DisplayAtkOrDef(_item);
                Console.WriteLine($" {_item.ItemComm} ");
            }
            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기");
            int _input = CheckValidAction(0, _itemsInDatabase.Count);

            if (_input == 0)
            {
                Console.Clear();
                DisplayPlayerInventory();
            }
            else if (_input > 0 && _input <= _itemsInDatabase.Count)
            {
                ItemData _selectedItem = _itemsInDatabase[_input - 1];
                ToggleEquip(_selectedItem);
                ManagementPlayerInventory();
            }
        }

        static void ToggleEquip(ItemData _item)
        {
            bool _isAtkItemEqupped = IsAtkItemEquipped();
            bool _isDefItemEquipped = IsDefItemEquipped();

            if (_item.ItemAtk > 0 && _isAtkItemEqupped && !_item.IsItemEquipped)
            {
                return;
            }

            if (_item.ItemDef > 0 && _isDefItemEquipped && !_item.IsItemEquipped)
            {
                return;
            }

            _item.IsItemEquipped = !_item.IsItemEquipped;

            if (_item.IsItemEquipped)
            {
                _playerEquippedItems.Add(_item);
            }
            else
            {
                _playerEquippedItems.Remove(_item);
            }
            // 만약 이미 장착한 아이템이 있다면? -> ItemAtk
            

            UpdatePlayerStats();
        }

        static void UpdatePlayerStats()
        {
            int _totalAtk = 0;
            int _totalDef = 0;

            foreach (ItemData _item in _playerEquippedItems)
            {
                _totalAtk += _item.ItemAtk;
                _totalDef += _item.ItemDef;
            }

            _playerStat.AtkValue = _playerStat.BaseAtkValue + _totalAtk;
            _playerStat.DefValue = _playerStat.BaseDefValue + _totalDef;
        }

        static void DisplayAtkOrDef(ItemData _item)
        {
            if (_item.ItemAtk > 0 && _item.ItemDef == 0)
            {
                Console.Write($"| 공격력 + {_item.ItemAtk} |");
            }
            else if (_item.ItemAtk == 0 && _item.ItemDef > 0)
            {
                Console.Write($"| 방어력 + {_item.ItemDef} |");
            }
        }

        static bool IsAtkItemEquipped()
        {
            foreach (ItemData _tem in _playerEquippedItems)
            {
                if (_tem.ItemAtk > 0)
                {
                    return true;
                }
            }
            return false;
        }

        static bool IsDefItemEquipped()
        {
            foreach (ItemData _item in _playerEquippedItems)
            {
                if (_item.ItemDef > 0)
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
            Console.WriteLine($"Lv. {_playerStat.Level}");
            Console.WriteLine($"{_playerStat.Name} ( {_playerStat.PlayerClass} )");
            Console.WriteLine($"공격력 : {_playerStat.AtkValue}");
            Console.WriteLine($"방어력 : {_playerStat.DefValue}");
            Console.WriteLine($"체 력 : {_playerStat.HpValue}"); ;
            Console.WriteLine($"Gold : {_playerStat.Gold} G");
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
        public string Name;
        public string PlayerClass;
        public int Level;
        public int AtkValue;
        public int DefValue;
        public int HpValue;
        public int Gold;
        public int BaseAtkValue;
        public int BaseDefValue;

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
        public int ItemId;
        public bool IsItemEquipped;
        public string ItemName;
        public int ItemAtk;
        public int ItemDef;
        public string ItemComm;

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