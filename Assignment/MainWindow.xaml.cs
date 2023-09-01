using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClearData();
            ShowDataOnListView();
        }

        private void ClearData()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            radNam.IsChecked = true;
            txtDienThoai.Clear();
            dtpNgayVaoLam.SelectedDate = DateTime.Now;
            radBanHang.IsChecked = true;
            txtDoanhSo.Clear();
            txtPCNhienLieu.Clear();
            txtMaNV.Focus();
        }

        private void ShowDataOnListView()
        {
            string strConn = "server = DESKTOP-I2994Q8\\CONGVINH;database = QuanLyNhanVien;uid=sa;pwd=0762730979manh";
            SqlConnection con = new SqlConnection(strConn);
            //mở kết nối
            if (con.State == ConnectionState.Closed) con.Open();
            //tạo câu truy vấn
            string sql = "SELECT * FROM Employee WHERE IsDeleted IS null";
            //gọi đối tượng SQLcommand
            SqlCommand command = new SqlCommand(sql, con);
            //gọi đối tượng SQLDatareader
            SqlDataReader reader = command.ExecuteReader();
            //Clear listview        
            while (reader.Read())
            {
                string empid = reader.GetString(1);
                string empname = reader.GetString(2);
                string gender = reader.GetString(3);
                string phone = reader.GetString(4);
                DateTime time = reader.GetDateTime(5);
                string emptype = reader.GetString(6);
                double sales = reader.GetDouble(7);
                double allowance = reader.GetDouble(8);

                QuanLy emp = new QuanLy()
                {
                    EmpId = empid,
                    EmpName = empname,
                    Gender = gender,
                    Phone = phone,
                    WorkingDate = time,   
                    EmpType = emptype,
                    Sales = sales,
                    Allowance = allowance,
                };
                //Hiển thị data lên listview
                lvDanhSach.Items.Add(emp);               
            }
            reader.Close();
            con.Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn chắc chắn muốn thoát hả?", "Xác nhận thoát", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // Cancel closing
            }
        }


        private void radGiaoNhan_Checked(object sender, RoutedEventArgs e)
        {
            txtDoanhSo.Visibility = Visibility.Collapsed;
            lblDoanhSo.Visibility = Visibility.Collapsed;
            txtPCNhienLieu.Visibility = Visibility.Visible;
            lblPCNhienLieu.Visibility = Visibility.Visible;

        }

        private void radBanHang_Checked(object sender, RoutedEventArgs e)
        {
            txtDoanhSo.Visibility = Visibility.Visible;
            lblDoanhSo.Visibility = Visibility.Visible;
            txtPCNhienLieu.Visibility = Visibility.Collapsed;
            lblPCNhienLieu.Visibility = Visibility.Collapsed;

        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {        
            ClearData();

        }
        private bool IsValidData()
        {
            string maNV = txtMaNV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string doanhSo = txtDoanhSo.Text.Trim();
            string phuCap = txtPCNhienLieu.Text.Trim();
            DateTime ngayVaoLam = dtpNgayVaoLam.SelectedDate ?? DateTime.Now;


            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Mã nhân viên chưa được nhập!", "Lỗi dữ liệu", MessageBoxButton.OK);
                return false;
            }

            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Tên nhân viên chưa được nhập!.", "Lỗi dữ liệu", MessageBoxButton.OK);
                return false;
            }

            if (radBanHang.IsChecked == true && string.IsNullOrEmpty(doanhSo))
            {
                MessageBox.Show("Doanh số chưa được nhập!", "Lỗi dữ liệu", MessageBoxButton.OK);
                return false;
            }

            if (radGiaoNhan.IsChecked == true && string.IsNullOrEmpty(phuCap))
            {
                MessageBox.Show("Phụ Cấp Nhiên Liệu chưa được nhập!", "Lỗi dữ liệu", MessageBoxButton.OK);
                return false;
            }
            if (ngayVaoLam > DateTime.Now)
            {
                MessageBox.Show("Ngày vào làm không được lớn hơn ngày hiện tại!");
                return false;
            }
            return true;
        }
        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
                        
            if (IsValidData())
            {
                string strConn = "server = DESKTOP-I2994Q8\\CONGVINH;database = QuanLyNhanVien;uid=sa;pwd=0762730979manh";
                SqlConnection con = new SqlConnection(strConn);
                //mở kết nối
                if (con.State == ConnectionState.Closed) con.Open();
                //tạo câu truy vấn
                string insertsql = "INSERT INTO Employee(EmpId,EmpName,Gender,Phone,WorkingDate,EmpType,Sales,Allowance)" +
                    "VALUES (@EmpId,@EmpName,@Gender,@Phone,@WorkingDate,@EmpType,@Sales,@Allowance)";
                // Gọi đối tượng SqlCommand và thêm các tham số
                using (SqlCommand command = new SqlCommand(insertsql, con))
                {
                    command.Parameters.AddWithValue("@EmpId", txtMaNV.Text);
                    command.Parameters.AddWithValue("@EmpName", txtHoTen.Text);
                    command.Parameters.AddWithValue("@Gender", radNam.IsChecked == true ? "Nam" : "Nữ");
                    command.Parameters.AddWithValue("@Phone", txtDienThoai.Text);
                    command.Parameters.AddWithValue("@WorkingDate", dtpNgayVaoLam.SelectedDate);
                    command.Parameters.AddWithValue("@EmpType", radBanHang.IsChecked == true ? "Bán hàng" : "Giao nhận");
                    command.Parameters.AddWithValue("@Sales", txtDoanhSo.Text);
                    command.Parameters.AddWithValue("@Allowance", txtPCNhienLieu.Text);


                    // Thực thi truy vấn chèn dữ liệu
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Dữ liệu đã được lưu!");
                    }
                    else
                    {
                        MessageBox.Show("Lưu dữ liệu thất bại!");
                    }
                }
                ShowDataOnListView();
            }
            
        }
        private void lvDanhSach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDanhSach.SelectedItem != null)
            {

                QuanLy selectedEmp = lvDanhSach.SelectedItem as QuanLy;

                txtMaNV.Text = selectedEmp.EmpId.ToString();
                txtHoTen.Text = selectedEmp.EmpName.ToString();

                dtpNgayVaoLam.SelectedDate = selectedEmp.WorkingDate;
                txtDienThoai.Text = selectedEmp.Phone;

                if (selectedEmp.Gender == "Nam")
                    radNam.IsChecked = true;
                else
                    radNu.IsChecked = true;
                if (selectedEmp.EmpType == "Bán hàng")
                {
                    radBanHang.IsChecked = true;
                    txtDoanhSo.Text = selectedEmp.Sales.ToString();
                }
                else if (selectedEmp.EmpType == "Giao nhận")
                {
                    radGiaoNhan.IsChecked = true;
                    txtPCNhienLieu.Text = selectedEmp.Allowance.ToString();
                }
            }
            else
            {
                ClearData();
            }
        }
        
        private ObservableCollection<QuanLy> empList = new ObservableCollection<QuanLy>();
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (lvDanhSach.SelectedItem != null)
            {
                QuanLy empselected = (QuanLy)lvDanhSach.SelectedItem;

                // Update the employee's information
                empselected.EmpName = txtHoTen.Text;
                empselected.EmpId = txtMaNV.Text;
                empselected.Phone = txtDienThoai.Text;


                empselected.Gender = (bool)radNam.IsChecked ? "Nam" : "Nữ";
                empselected.EmpType = (bool)radBanHang.IsChecked ? "Bán hàng" : "Giao nhận";


                empselected.WorkingDate = dtpNgayVaoLam.SelectedDate.Value;

                if (empselected.EmpType == "Bán hàng")
                {
                    if (double.TryParse(txtDoanhSo.Text, out double doanhSo))
                    {
                        empselected.Sales = doanhSo;
                    }
                    else
                    {
                        MessageBox.Show("Dữ liệu Doanh số không hợp lệ.");
                        return;
                    }
                }


                else if (empselected.EmpType == "Giao nhận")
                {
                    empselected.Allowance = double.Parse(txtPCNhienLieu.Text);
                    if (double.TryParse(txtPCNhienLieu.Text, out double phuCap))
                    {
                        empselected.Allowance = phuCap;
                    }
                    else
                    {
                        MessageBox.Show("Dữ liệu Phụ cấp không hợp lệ.");
                        return;
                    }
                }

                // Update the changes to the database
                string strConn = "server = DESKTOP-I2994Q8\\CONGVINH;database = QuanLyNhanVien;uid=sa;pwd=0762730979manh";
                SqlConnection con = new SqlConnection(strConn);
                //mở kết nối
                if (con.State == ConnectionState.Closed) con.Open();
                //tạo câu truy vấn
                string insertsql = "UPDATE Employee SET EmpId = @MaNV, EmpName = @HoTen, " +
                    "Gender = @GioiTinh, Phone = @Phone, WorkingDate = @NgayVaoLam, EmpType = @Type, " +
                    "Sales = @DoanhSo, Allowance = @PhuCap where EmpID = @MaNV";
                // Gọi đối tượng SqlCommand và thêm các tham số
                using (SqlCommand command = new SqlCommand(insertsql, con))
                {
                    command.Parameters.AddWithValue("@MaNV", txtMaNV.Text);
                    command.Parameters.AddWithValue("@HoTen", txtHoTen.Text);
                    command.Parameters.AddWithValue("@GioiTinh", radNam.IsChecked == true ? "Nam" : "Nữ");
                    command.Parameters.AddWithValue("@Phone", txtDienThoai.Text);
                    command.Parameters.AddWithValue("@NgayVaoLam", dtpNgayVaoLam.SelectedDate);
                    command.Parameters.AddWithValue("@Type", radBanHang.IsChecked == true ? "Bán hàng" : "Giao nhận");
                    command.Parameters.AddWithValue("@DoanhSo", txtDoanhSo.Text);
                    command.Parameters.AddWithValue("@PhuCap", txtPCNhienLieu.Text);

                    
                    // Thực thi truy vấn chèn dữ liệu
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật dữ liệu thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật dữ liệu thất bại!");
                    }
                }
            }

            // Refresh the ListView to show the updated information
            lvDanhSach.Items.Refresh();

        }

        private void btnSapXep_Click(object sender, RoutedEventArgs e)
        {
            // Sort the items in the ListView
            lvDanhSach.Items.SortDescriptions.Clear();
            lvDanhSach.Items.SortDescriptions.Add(new SortDescription("WorkingYear",
                ListSortDirection.Descending));
            lvDanhSach.Items.SortDescriptions.Add(new SortDescription("EmpName",
                ListSortDirection.Ascending));
        }

        private void btnThongke_Click(object sender, RoutedEventArgs e)
        {
            int countSales = 0;
            int countAllowance = 0;
            double sumSales = 0;
            double sumAllowance = 0;
            double salary = 7000000;

            foreach (QuanLy emp in lvDanhSach.Items)
            {
                if (emp.EmpType == "Bán hàng")
                {
                    countSales++;
                    sumSales += salary + emp.Sales * 0.1;
                }
                else if (emp.EmpType == "Giao nhận")
                {
                    countAllowance++;
                    sumAllowance += salary + emp.Allowance;
                }
            }

            MessageBox.Show("Công ty hiện có " + countSales + " nhân viên bán hàng, "
                + countAllowance + " nhân viên giao nhận.\nTổng lương chi cho nhân viên bán hàng: "
                + sumSales.ToString("C0")
                + "\nTổng lương chi cho nhân viên giao nhận: "
                + sumAllowance.ToString("C0"));
        }

        private void btnXoa_Click_1(object sender, RoutedEventArgs e)
        {
            if (lvDanhSach.SelectedItem != null)
            {
                QuanLy selectedEmp = (QuanLy)lvDanhSach.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Bạn chắc chắn muốn xóa nhân viên hả?", "Xác nhận xóa", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    string strConn = "server = DESKTOP-I2994Q8\\CONGVINH;database = QuanLyNhanVien;uid=sa;pwd=0762730979manh";
                    SqlConnection con = new SqlConnection(strConn);
                    try
                    {
                        //mở kết nối
                        if (con.State == ConnectionState.Closed) con.Open();
                        //tạo câu truy vấn
                        string sql = "UPDATE Employee SET IsDeleted = 1 WHERE EmpId = @MaNV";
                        //gọi đối tượng SQLcommand

                        using (SqlCommand command = new SqlCommand(sql, con))
                        {
                            command.Parameters.AddWithValue("@MaNV", selectedEmp.EmpId);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Dữ liệu đã được xóa thành công!");
                                // Xóa nhân viên khỏi danh sách
                                empList.Remove(selectedEmp);
                                ShowDataOnListView();
                            }
                            else
                            {
                                MessageBox.Show("Xóa dữ liệu thất bại!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open) con.Close();
                    }
                    if (lvDanhSach.Items.Count > 0)
                    {
                        lvDanhSach.SelectedIndex = 0;
                    }
                    else
                    {
                        // Reset the form to its default state
                        txtHoTen.Text = "";
                        txtMaNV.Text = "";
                        txtDienThoai.Text = "";
                        txtDoanhSo.Text = "";
                        txtPCNhienLieu.Text = "";
                        radNam.IsChecked = true;
                        radNu.IsChecked = false;
                        radBanHang.IsChecked = true;
                        radGiaoNhan.IsChecked = false;
                        dtpNgayVaoLam.SelectedDate = DateTime.Today;
                    }
                }
            }
        }
    }
}
