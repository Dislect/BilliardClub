using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;

namespace BilliardClub.Models.Initialization
{
    public class InitialDataBase
    {
        private static string AmericanPoolTable = "Американский пул";
        private static string RussianPoolTable = "Русский бильярд";

        public static async Task InitialAsync(Context _context)
        {
            if (!_context.TypeTables.Any())
            {
                _context.TypeTables.AddRange(
                    new TypeTable()
                    {
                        name = RussianPoolTable,
                        price = 200
                    },

                    new TypeTable()
                    {
                        name = AmericanPoolTable,
                        price = 200
                    }
                );
                await _context.SaveChangesAsync();
            }

            if (!_context.TableRotations.Any())
            {
                _context.TableRotations.AddRange(
                    new TableRotation()
                    {
                        rotationAngle = 0
                    },

                    new TableRotation()
                    {
                        rotationAngle = 90
                    }
                ); 
                await _context.SaveChangesAsync();
            }

            if (!_context.PoolTables.Any())
            {
                _context.PoolTables.AddRange(
                    new PoolTable()
                    {
                        name = "1",
                        tableX = 130,
                        tableY = 150,
                        tableRotation = _context.TableRotations.FirstOrDefault(),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "2",
                        tableX = 290,
                        tableY = 150,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "3",
                        tableX = 130,
                        tableY = 320,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    }, 

                    new PoolTable()
                    {
                        name = "4",
                        tableX = 290,
                        tableY = 315,
                        tableRotation = _context.TableRotations.FirstOrDefault(),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "5",
                        tableX = 465,
                        tableY = 315,
                        tableRotation = _context.TableRotations.FirstOrDefault(),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "6",
                        tableX = 130,
                        tableY = 475,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "7",
                        tableX = 290,
                        tableY = 480,
                        tableRotation = _context.TableRotations.FirstOrDefault(),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "8",
                        tableX = 465,
                        tableY = 475,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "9",
                        tableX = 290,
                        tableY = 625,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "10",
                        tableX = 465,
                        tableY = 630,
                        tableRotation = _context.TableRotations.FirstOrDefault(),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "VIP",
                        tableX = 200,
                        tableY = 1110,
                        tableRotation = _context.TableRotations.FirstOrDefault(),
                        typeTable = _context.TypeTables.FirstOrDefault()
                    },

                    new PoolTable()
                    {
                        name = "11",
                        tableX = 635,
                        tableY = 630,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "12",
                        tableX = 805,
                        tableY = 630,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "13",
                        tableX = 970,
                        tableY = 630,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "14",
                        tableX = 635,
                        tableY = 780,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "15",
                        tableX = 805,
                        tableY = 780,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "16",
                        tableX = 970,
                        tableY = 780,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "17",
                        tableX = 635,
                        tableY = 935,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "18",
                        tableX = 805,
                        tableY = 935,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "19",
                        tableX = 970,
                        tableY = 935,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "20",
                        tableX = 715,
                        tableY = 1110,
                        tableRotation = _context.TableRotations.FirstOrDefault(),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "21",
                        tableX = 895,
                        tableY = 1110,
                        tableRotation = _context.TableRotations.FirstOrDefault(),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "22",
                        tableX = 715,
                        tableY = 1270,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    },

                    new PoolTable()
                    {
                        name = "23",
                        tableX = 878,
                        tableY = 1270,
                        tableRotation = _context.TableRotations.FirstOrDefault(x => x.rotationAngle == 90),
                        typeTable = _context.TypeTables.FirstOrDefault(x => x.name == AmericanPoolTable)
                    }
                );
            }

            if (!_context.Status.Any())
            {
                _context.Status.AddRange(
                    new Status()
                    {
                        name = "Забронирован"
                    },

                    new Status()
                    {
                        name = "В ремонте"
                    },

                    new Status()
                    {
                        name = "В корзине"
                    },

                    new Status()
                    {
                        name = "Свободен"
                    },

                    new Status()
                    {
                        name = "Забронирован к дате"
                    }
                );
            }

            if (!_context.FoodItems.Any())
            {
                _context.FoodItems.AddRange(
                    new FoodItem()
                    {
                        title = "Коктейль",
                        price = 350,
                        picturePath = "/img/catalog/cocktail.jpg"
                    },

                    new FoodItem()
                    {
                        title = "Пицца",
                        price = 500,
                        picturePath = "/img/catalog/pizza.png"
                    },

                    new FoodItem()
                    {
                        title = "Закуска",
                        price = 550,
                        picturePath = "/img/catalog/snack.jpg"
                    }
                );
            }
            await _context.SaveChangesAsync();
        }
    }
}