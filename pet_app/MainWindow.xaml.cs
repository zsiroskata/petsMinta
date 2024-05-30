using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace pet_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Pet> UnsortedPets;
        ObservableCollection<Pet> Pets;
        ObservableCollection<Pet> FavouritePets;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ObservableCollection<Pet> UnsortedPets = new();
            //így is jó lenne:
            //List<Pet> UnsortedPets = new();
            //Az ObservableCollection<T> egy speciális lista típus a.NET-ben, amely többet tud, mint a hagyományos List<T>.
            //Az ObservableCollection< T > fő előnye, hogy képes értesítéseket küldeni, amikor változás történik a gyűjteményben (például elem hozzáadása, törlése vagy módosítása).
            //Ez nagyon hasznos a data binding esetében, különösen a WPF és más XAML - alapú technológiákban, ahol a felhasználói felületnek automatikusan frissülnie kell, amikor az adatok megváltoznak.
            //Például, ha egy ObservableCollection < T > -t kötsz egy ListBox - hoz, és hozzáadsz egy elemet az ObservableCollection<T>-hez, a ListBox automatikusan frissül, és megjeleníti az új elemet. Ezt a List<T> nem tudja megtenni. Lásd lent a hozzáadásnál.

            var data = File.ReadAllLines(@"..\..\..\src\animals.txt");

            foreach (var item in data)
            {
                var d = item.Split(";");
                string name = d[0];
                int age = int.Parse(d[1]);
                string color = d[2];
                string source = d[3];

                UnsortedPets.Add(new Pet(name, age, color, source));
            }
            Pets = new ObservableCollection<Pet>(UnsortedPets.OrderBy(p => p.Name));
            FavouritePets = new();


            petek.ItemsSource = Pets;
            petek.SelectedItem = Pets[0];
            petek.SelectionChanged += petek_selectionChanged;
           
            kedvencek.ItemsSource = FavouritePets;
        }
        private void petek_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (petek.SelectedItem != null)
            {
                UpdateAttributeText();
            }
        }
        //A képfájlok betöltéséhez a legbiztosabb módszer az, ha azokat beágyazott erőforrásként(embedded resource) adod hozzá a projekthez, és a Pack URI sémát használod az elérési út megadásához.
        //    Azonban ha ragaszkodsz a relatív elérési út használatához, akkor a Directory.GetCurrentDirectory() metódussal lekérdezheted a jelenlegi munkakönyvtárat, és ennek segítségével állíthatod be a képfájlok helyét.

        private void UpdateAttributeText()
        {
            if (petek.SelectedItem != null)
            {
                Pet selectedItem = (Pet)petek.SelectedItem;
                attributeTextBlock.Text = $"Megnevezés: {selectedItem.Name}\n" +
                                          $"Életkor: {selectedItem.Age} \n" +
                                          $"Szín: {selectedItem.Color}";
                Image finalImage = new();
                
                string currentDirectory = Directory.GetCurrentDirectory();
                finalImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(currentDirectory, $@"..\Images\{selectedItem.Image}")));


                imageContainer.Children.Clear(); // Először töröljük az előző képet
                imageContainer.Children.Add(finalImage); // Majd hozzáadjuk az új képet
            }
        }
        private void btn_torol_Click(object sender, RoutedEventArgs e)
        {

            if (petek.SelectedItem != null)
            {
                //kedvencek.Items.Remove(kedvencek.SelectedItem);//itt a listából kell törölni, nem a controlból
                FavouritePets.Remove((Pet)kedvencek.SelectedItem);
            }

        }

        private void btn_kedvencek_Click(object sender, RoutedEventArgs e)
        {
            if (petek.SelectedItem != null && !kedvencek.Items.Contains(petek.SelectedItem))
            {
                //adatkötés nélkül (az XAML-ből ilyenkor ki kell venni a jelölt részeket):
                //ez is jó:
                //kedvencek.Items.Add((Pet)petek.SelectedItem); //ToString override miatt jó

                //ez is jó:
                //var nev = ((Pet)petek.SelectedItem).Name;
                //kedvencek.Items.Add(nev);


                //adatkötéssel:
                //ez is jó:
                FavouritePets.Add((Pet)petek.SelectedItem); //ha a listát kezelem - megtehetem, mert ő egy ObservableCollection

                //ez is jó:
                //kedvencek.Items.Add(petek.SelectedItem); //ha a controlt kezelem
            }
        }
    }
}