namespace SpartaDungeonPractice
{
    internal class Program
    {
        private static PlayerStat playerStat;
        private static ItemData itemData;
        static List<ItemData> itemList = new List<ItemData>();

        static void Main(string[] args)
        {
            PlayerDataSet();
            ItemListSet();
            MainGameScene();

        }
        static void PlayerDataSet()
        {
            Console.WriteLine("게임에 사용하실 닉네임을 입력해주세요!");
            Console.Write(">>");
            // 게임 닉네임 설정 및 데이터 설정
            string _inputName = Console.ReadLine();

            if (_inputName != null)
            {
                Console.Clear();
                playerStat = new PlayerStat($"{_inputName}", "전사", 1, 10, 5, 100, 500);
            }
            else
            {
                Console.WriteLine("이름을 입력해주세요!");
            }
        }

        static void ItemListSet()
        {
            itemData = new ItemData("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.");
            itemList.Add(itemData);
            itemData = new ItemData("천 갑옷", 0, 2, "질긴 천을 덧대어 만든 낡은 천갑옷 입니다.");
            itemList.Add(itemData);
        }

        static void MainGameScene()
        {
            Console.WriteLine("Sparta Dungeon Game!");
            Console.WriteLine($"{playerStat.Name}님, 스파르타 마을에 오신것을 환영합니다!");
            Console.WriteLine("이곳에서 던전으로 돌아가기 전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine(" ");
            int _input = CheckValidAction(1, 2);

            switch (_input)
            {
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
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");

            foreach (ItemData item in itemList)
            {
                Console.Write($" {item.ItemName} ");
                DisplayAtkorDef();
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
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");
            

            foreach (ItemData item in itemList)
            {
                string itemEquipped = itemData.IsItemEquipped ? "[E]" : "";
                Console.Write($"{itemEquipped}{item.ItemName}");
                DisplayAtkorDef();
                Console.WriteLine($" {item.ItemComm} ");
            }
            Console.WriteLine(" ");
            Console.WriteLine("0. 나가기");
            int _input = CheckValidAction(0, 2);
            switch (_input)
            {
                case 0:
                    DisplayPlayerInventory();
                    break;
                case 1:
                    itemList[0].IsItemEquipped = !itemList[0].IsItemEquipped; // 첫 번째 아이템의 장착 상태 변경
                    ManagementPlayerInventory();
                    break;
                case 2:
                    itemList[1].IsItemEquipped = !itemList[1].IsItemEquipped; // 두 번째 아이템의 장착 상태 변경
                    ManagementPlayerInventory();
                    break;
            }

        }

        static void DisplayAtkorDef()
        {
            if(itemData.ItemAtk > 0 && itemData.ItemDef == 0)
            {
                Console.Write($"| 공격력 + {itemData.ItemAtk} |");
            }
            else if (itemData.ItemAtk == 0 && itemData.ItemDef > 0)
            {
                Console.Write($"| 방어력 + {itemData.ItemDef} |");
            }
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

        public PlayerStat(string _name, string _playerClass, int _level, int _atkValue, int _defValue, int _hpValue, int _gold)
        {
            Name = _name;
            PlayerClass = _playerClass;
            Level = _level;
            AtkValue = _atkValue;
            DefValue = _defValue;
            HpValue = _hpValue;
            Gold = _gold;
        }
    }

    public class ItemData
    {
        public string ItemName { get; set; }
        public int ItemAtk { get; set; }
        public int ItemDef { get; set; }
        public bool IsItemEquipped { get; set; }
        public string ItemComm { get; set; }

        public ItemData(string _itemName, int _itemAtk, int _itemDef, string _itemComm)
        {
            ItemName = _itemName;
            ItemAtk = _itemAtk;
            ItemDef = _itemDef;
            ItemComm = _itemComm;
            IsItemEquipped = false;
        }
    }
}