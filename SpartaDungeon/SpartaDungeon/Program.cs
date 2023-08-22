namespace SpartaDungeo
    internal class Program
    {
        private static PlayerStat _playerStat;
        private static ItemData _itemData;
        static List<ItemData> _itemsInDatabase = new List<ItemData>();
        static List<ItemData> _playerEquippedItems = new List<ItemData>();

        static void Main(string[] args)
        {
            // witdht, height
            Console.SetWindowSize(200, 50);
            InitItemDatabase();
            PlayerDataSet();
            MainGameScene();
        }

        static void InitItemDatabase()
        {
            _itemsInDatabase.Add(new ItemData(0, "낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검입니다.", 200, true));
            _itemsInDatabase.Add(new ItemData(1, "천 갑옷", 0, 2, "질긴 천을 덧대어 제작한 낡은 갑옷입니다.", 150, true));
            _itemsInDatabase.Add(new ItemData(2, "헤라클레스의 곤봉", 5, 0, "이 곤봉은 12가지 과업을 대비해서 갖고 다녀야합니다.", 500, false));
            _itemsInDatabase.Add(new ItemData(3, "포세이돈의 삼지창", 10, 0, "이 삼지창을 쥐면 바다를 다스릴 수 있습니다.", 1000, false));
            _itemsInDatabase.Add(new ItemData(4, "신령의 갑옷", 0, 15, "신비한 힘이 깃든 갑옷", 1300, false));
            _itemsInDatabase.Add(new ItemData(5, "트리스메기투스의 지팡이", 30, 0, "미지의 세계, 아틀란티스로 갈 수 있는 열쇠입니다.", 3000, false));
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
            Console.WriteLine("3. 상점");
            Console.WriteLine(" ");
            int input = CheckValidAction(0, 3);

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
                case 3:
                    DisplayItemShop();
                    break;
            }
        }
        static void DisplayItemShop()
        {
            Console.Clear();
            Console.Title = "상점";
            Console.WriteLine("[보유 골드]\n");
            Console.WriteLine($"{_playerStat.Gold} G\n");
            Console.WriteLine("[상점 아이템 목록]\n");

            for (int i = 0; i < _itemsInDatabase.Count; i++)
            {
                ItemData _shopItem = _itemsInDatabase[i];
                string _itemName = FormatAndPad(_shopItem.ItemName, 17);
                string _itemComm = FormatAndPad(_shopItem.ItemComm, 40);

                if (!_shopItem.IsPlayerOwned)
                {
                    Console.WriteLine($"{i + 1} | {_itemName} | {_itemComm} | 구매 가격 : {_shopItem.ItemPrice} G");
                }
            }
            Console.WriteLine(" ");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");

            int _input = CheckValidAction(0, 2);

            switch (_input)
            {
                case 0:
                    Console.Clear();
                    MainGameScene();
                    break;
                case 1:
                    Console.Clear();
                    BuyManagementItemShop();
                    break;
                case 2:
                    Console.Clear();
                    SellManagementItemShop();
                    break;
            }
        }

        static void BuyManagementItemShop()
        {
            Console.Clear();
            Console.Title = "상점";
            Console.WriteLine("[보유 골드]\n");
            Console.WriteLine($"{_playerStat.Gold} G\n");
            Console.WriteLine("[상점 아이템 목록]\n");

            for (int i = 0; i < _itemsInDatabase.Count; i++)
            {
                ItemData _shopItem = _itemsInDatabase[i];
                string _itemName = FormatAndPad(_shopItem.ItemName, 17);
                string _itemComm = FormatAndPad(_shopItem.ItemComm, 40);

                if (!_shopItem.IsPlayerOwned)
                {
                    Console.WriteLine($"{i + 1} | {_itemName} | {_itemComm} | 구매 가격 : {_shopItem.ItemPrice} G");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("[인벤토리 아이템 목록]\n");

            for (int i = 0; i < _itemsInDatabase.Count; i++)
            {
                ItemData _inventoryItem = _itemsInDatabase[i];
                if (_inventoryItem.IsPlayerOwned)
                {
                    Console.WriteLine($"{i + 1} | {_inventoryItem.ItemName} | 판매 가격 :{_inventoryItem.ItemPrice * 0.8} G | 소지중");
                }
            }

            Console.WriteLine("");
            Console.WriteLine("0. 나가기\n");

            Console.WriteLine("아이템의 고유 번호를 입력해주세요.");
            int _input = CheckValidAction(0, _itemsInDatabase.Count);

            if (_input == 0)
            {
                Console.Clear();
                DisplayItemShop();
            }
            else
            {
                // 입력한 번호에 해당하는 아이템의 인덱스 계산
                int _itemIndex = _input - 1;

                if (!_itemsInDatabase[_itemIndex].IsPlayerOwned)
                {
                    BuyItem(_itemIndex); // 아이템을 구매
                }
                // 상점 목록을 다시 출력
                BuyManagementItemShop();
            }
        }

        static void SellManagementItemShop()
        {
            Console.Clear();
            Console.Title = "상점";
            Console.WriteLine("[보유 골드]\n");
            Console.WriteLine($"{_playerStat.Gold} G\n");
            Console.WriteLine("[상점 아이템 목록]\n");

            for (int i = 0; i < _itemsInDatabase.Count; i++)
            {
                ItemData _shopItem = _itemsInDatabase[i];
                string _itemName = FormatAndPad(_shopItem.ItemName, 17);
                string _itemComm = FormatAndPad(_shopItem.ItemComm, 40);

                if (!_shopItem.IsPlayerOwned)
                {
                    Console.WriteLine($"{i + 1} | {_itemName} | {_itemComm} | 구매 가격 : {_shopItem.ItemPrice} G");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("[인벤토리 아이템 목록]\n");

            for (int i = 0; i < _itemsInDatabase.Count; i++)
            {
                ItemData _inventoryItem = _itemsInDatabase[i];
                if (_inventoryItem.IsPlayerOwned)
                {
                    Console.WriteLine($"{i + 1} | {_inventoryItem.ItemName} | 판매 가격 :{_inventoryItem.ItemPrice * 0.8} G | 소지중");
                }
            }

            Console.WriteLine("");
            Console.WriteLine("0. 나가기\n");

            Console.WriteLine("아이템의 고유 번호를 입력해주세요.");
            int _input = CheckValidAction(0, _itemsInDatabase.Count);

            if (_input == 0)
            {
                Console.Clear();
                DisplayItemShop();
            }
            else
            {
                // 입력한 번호에 해당하는 아이템의 인덱스 계산
                int _itemIndex = _input - 1;

                if (_itemsInDatabase[_itemIndex].IsPlayerOwned == true)
                {
                    Sell_Item(_itemIndex); // 아이템을 판매
                }
                // 상점 목록을 다시 출력
                SellManagementItemShop();
            }
        }

        static void BuyItem(int _itemIndex)
        {
            ItemData _selectedShopItem = _itemsInDatabase[_itemIndex];

            if (_playerStat.Gold >= _selectedShopItem.ItemPrice)
            {
                _playerStat.Gold -= _selectedShopItem.ItemPrice;
                _selectedShopItem.IsPlayerOwned = true;
                Console.WriteLine("");
                Console.WriteLine($"{_selectedShopItem.ItemName}을(를) 구매하였습니다.\n");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("골드가 부족합니다.");
            }
            Console.WriteLine("아무 키나 입력하세요...\n"); // 사용자의 입력을 기다림
            Console.ReadKey(); // 아무 키나 입력할 때까지 대기
        }

        static void Sell_Item(int _itemIndex)
        {
            ItemData _selectedShopItem = _itemsInDatabase[_itemIndex];
            if (_selectedShopItem.IsPlayerOwned == true)
            {
                double _sellRet = _selectedShopItem.ItemPrice * 0.8;
                _playerStat.Gold += (int)_sellRet;
                _selectedShopItem.IsPlayerOwned = false;
                Console.WriteLine($"{_selectedShopItem.ItemName}을(를) 판매하였습니다.\n");
            }
            Console.WriteLine("아무 키나 입력하세요...\n"); // 사용자의 입력을 기다림
            Console.ReadKey(); // 아무 키나 입력할 때까지 대기
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

                if (!_item.IsPlayerOwned)
                {
                    continue; // IsPlayerOwned가 false면 출력하지 않고 다음 아이템으로 넘어감
                }

                _itemEquipped = _item.IsItemEquipped ? "[E] " : "";

                string _itemName = FormatAndPad(_item.ItemName, 17);
                string _itemComm = FormatAndPad(_item.ItemComm, 30);

                Console.Write($"{_item.ItemId + 1} | ");
                if (_item.IsItemEquipped)
                {
                    SetConsoleColor(ConsoleColor.Yellow);
                }
                Console.Write($"{_itemEquipped}");
                Console.ResetColor();
                Console.Write($"{_itemName}");
                DisplayAtkOrDef(_item);
                Console.WriteLine($" {_itemComm} ");
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

                if (!_item.IsPlayerOwned)
                {
                    continue; // IsPlayerOwned가 false면 출력하지 않고 다음 아이템으로 넘어감
                }

                _itemEquipped = _item.IsItemEquipped ? "[E] " : "";
                string _itemName = FormatAndPad(_item.ItemName, 17);
                string _itemComm = FormatAndPad(_item.ItemComm, 30);
                Console.Write($"{_item.ItemId + 1} | ");
                if (_item.IsItemEquipped)
                {
                    SetConsoleColor(ConsoleColor.Yellow);
                }
                Console.Write($"{_itemEquipped}");
                Console.ResetColor();
                Console.Write($"{_itemName}");
                DisplayAtkOrDef(_item);
                Console.WriteLine($" {_itemComm} ");
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

        static string FormatAndPad(string _text, int _width)
        {
            int _remainingSpace = _width - _text.Length;
            if (_remainingSpace <= 0)
            {
                return _text;
            }
            else
            {
                int _leftPadding = _remainingSpace / 2;
                int _rightPadding = _remainingSpace - _leftPadding;
                string _formattedText = new string(' ', 2 * _leftPadding) + _text + new string(' ', 2 * _rightPadding);
                return _formattedText;
            }
        }

        static void ToggleEquip(ItemData _item)
        {
            bool _isAtkItemEquipped = IsAtkItemEquipped();
            bool _isDefItemEquipped = IsDefItemEquipped();

            if (_item.ItemAtk > 0 && _isAtkItemEquipped && !_item.IsItemEquipped)
            {
                for (int i = _playerEquippedItems.Count - 1; i >= 0; i--)
                {
                    ItemData item = _playerEquippedItems[i];
                    if (item.IsItemEquipped && item.ItemAtk > 0)
                    {
                        item.IsItemEquipped = false;
                        _playerEquippedItems.RemoveAt(i);
                        break;
                    }
                }
            }

            // 중복 방어구 아이템 제거 후 장착
            if (_item.ItemDef > 0 && _isDefItemEquipped && !_item.IsItemEquipped)
            {
                for (int i = _playerEquippedItems.Count - 1; i >= 0; i--)
                {
                    ItemData item = _playerEquippedItems[i];
                    if (item.IsItemEquipped && item.ItemDef > 0)
                    {
                        item.IsItemEquipped = false;
                        _playerEquippedItems.RemoveAt(i);
                        break;
                    }
                }
                //foreach (var item in _playerEquippedItems)
                //{
                //    if (item.IsItemEquipped && item.ItemDef > 0)
                //    {
                //        item.IsItemEquipped = false;
                //        _playerEquippedItems.Remove(item);
                //        break;
                //    }
                //}
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
                string _itemAtk = FormatAndPad(_item.ItemAtk.ToString(), 3);
                Console.Write($"| 공격력 + {_itemAtk} |");
            }
            else if (_item.ItemAtk == 0 && _item.ItemDef > 0)
            {
                string _itemDef = FormatAndPad(_item.ItemDef.ToString(), 3);
                Console.Write($"| 방어력 + {_itemDef} |");
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
        public bool IsPlayerOwned;
        public string ItemName;
        public int ItemAtk;
        public int ItemDef;
        public string ItemComm;
        public int ItemPrice { get; set; }

        public ItemData(int _itemId, string _itemName, int _itemAtk, int _itemDef, string _itemComm, int _itemPrice, bool _isPlayerOwned)
        {
            ItemId = _itemId;
            ItemName = _itemName;
            ItemAtk = _itemAtk;
            ItemDef = _itemDef;
            ItemComm = _itemComm;
            ItemPrice = _itemPrice;
            IsItemEquipped = false;
            IsPlayerOwned = _isPlayerOwned;
        }
    }
}