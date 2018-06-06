using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLBanHang
{
    public partial class frmMatHang : Form
    {
        SqlConnection con;//Khai bao doi tuong ket noi database
        SqlCommand cmd;//Khai bao doi tuong thuc hien cau lenh truy van
        SqlDataAdapter dap;//Khai bao doi tuong gan ket gatasource voi dataset
        DataSet ds;//Khai bao doi tuong chua du lieu
        public frmMatHang()
        {
            InitializeComponent();
        }

        private void frmMatHang_Load(object sender, EventArgs e)
        {
            //Tao doi tuong connect
            con = new SqlConnection();
            //Chuoi ket noi
            con.ConnectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\VuDat\Documents\Visual Studio 2013\Projects\QLBanHang\QLBanHang\QLBanHang\QLBanHang.mdf;Integrated Security=True;Connect Timeout=30";
            //Load du lieu
            LoadDuLieu("Select * from tblGear");
            //An cac nut Sua, Xoa khi load form
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //An groupbox chi tiet
            HienChiTiet(false);
        }
        //Ham nap du lieu len Datagrid
        private void LoadDuLieu(String sql)
        {
            //tao dataset
            ds = new DataSet();
            //tao Adapter
            dap = new SqlDataAdapter(sql,con);           
            dap.Fill(ds);
            //gan du lieu tu dataset len datagridview
            dgvKetQua.DataSource = ds.Tables[0];
        }
        //Tim kiem
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            //Cap nhat tieu de
            lblTieuDe.Text = "TÌM KIẾM MẶT HÀNG";
            //An nut sua va xoa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //Viet cau lenh SQL cho tim kiem
            String sql = "SELECT * FROM tblGear";
            String dk = "";
            //Tim theo MaSP khac rong
            if (txtTKMaSP.Text.Trim() != "")
            {
                dk += " masp like '%" + txtTKMaSP.Text + "%'";
            }
            //kiem tra TenSP va MaSP khac rong
            if (txtTKTenSP.Text.Trim() != "" && dk != "")
            {
                dk += " AND tensp like N'%" + txtTKTenSP.Text + "%'";
            }
            //Tim kiem theo TenSP khi MaSP la rong
            if (txtTKTenSP.Text.Trim() != "" && dk == "")
            {
                dk += " tensp like N'%" + txtTKTenSP.Text + "%'";
            }
            //Ket hoi dk
            if (dk != "")
            {
                sql += " WHERE" + dk;
            }
            //Load du lieu tim kiem
            LoadDuLieu(sql);   

        }
        //Add sp
        private void btnThem_Click(object sender, EventArgs e)
        {
            lblTieuDe.Text = "THÊM MẶT HÀNG";
            //Xoa trang GroupBox chi tiet san pham
            XoaTrangChiTiet();
            //Cam nut sua xoa
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            //hien groupbox chi tiet
            HienChiTiet(true);
        }
        //Xoa trang GroupBox chi tiet san pham
        private void XoaTrangChiTiet()
        {
            //Xoa trang cac control trong frame chi tiet
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtSoLuong.Clear();
            txtGia.Clear();
            txtThongSo.Clear();
        }
        //Phuong thuc an hien cac control trong group box chi tiet san pham
        private void HienChiTiet(Boolean hien)
        {
            txtMaSP.Enabled = hien;
            txtTenSP.Enabled = hien;           
            txtSoLuong.Enabled = hien;
            txtGia.Enabled = hien;
            txtThongSo.Enabled = hien;
            btnLuu.Enabled = hien;
            btnHuy.Enabled = hien;
        }
        //su kien chon 1 dong tren datagridview
        private void dgvKetQua_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Hien thi nut sua
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            //hien thi chi tiet san pham
            try
            {
                txtMaSP.Text = dgvKetQua[0, e.RowIndex].Value.ToString();
                txtTenSP.Text = dgvKetQua[1, e.RowIndex].Value.ToString();
                txtSoLuong.Text = dgvKetQua[2, e.RowIndex].Value.ToString();
                txtGia.Text = dgvKetQua[3, e.RowIndex].Value.ToString();
                txtThongSo.Text = dgvKetQua[4, e.RowIndex].Value.ToString();
            }
            catch (Exception ex)
            {
            }
        }
        //Su kien click nut Sua
        private void btnSua_Click(object sender, EventArgs e)
        {
            //cap nhat tieu de
            lblTieuDe.Text = "CẬP NHẬT MẶT HÀNG";
            //an hai nut them va xoa
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            //hien groupbox chi tiet
            HienChiTiet(true);
        }
        //su kien click nut Xoa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            //Message box canh bao
            if (MessageBox.Show("Bạn có chắc chắn xóa mã mặt hàng " + txtMaSP.Text + " không? Nếu có ấn nút Lưu, không thì ấn nút Hủy", "Xóa sản phẩm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lblTieuDe.Text = "XÓA MẶT HÀNG";
                btnThem.Enabled = false;
                btnSua.Enabled = false;

                HienChiTiet(true);
            }
        }
        //su kien click nut Luu
        private void btnLuu_Click(object sender, EventArgs e)
        {

            string sql = "";
            //kiem tra ket noi
            if (con.State != ConnectionState.Open)
                con.Open();
            //dung control ErrorProvider de hien thi loi
            //kiem tra trong thong tin
            if (txtTenSP.Text.Trim() == "")
            {
                errChiTiet.SetError(txtTenSP, "Bạn không để trống tên sản phẩm!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }
            //kiem tra trong thong tin
            if (txtSoLuong.Text.Trim() == "")
            {
                errChiTiet.SetError(txtSoLuong, "Bạn không để trống đơn vi!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }
            //kiem tra trong thong tin
            if (txtGia.Text.Trim() == "")
            {
                errChiTiet.SetError(txtGia, "Bạn không để trống đơn giá!");
                return;
            }
            else
            {
                errChiTiet.Clear();
            }

            //Them sp
            if (btnThem.Enabled == true)
            {
                //kiem tra trong thong tin
                if (txtMaSP.Text.Trim() == "")
                {
                    errChiTiet.SetError(txtMaSP, "Bạn không để trống mã sản phẩm trường này!");
                    return;
                }
                else
                {
                    //kiem tra masp co ton tai hay chua  
                    sql = "Select Count(*) From tblGear Where masp ='" + txtMaSP.Text + "'";
                    cmd = new SqlCommand(sql, con);
                    int val = (int)cmd.ExecuteScalar();
                    if (val > 0)
                    {
                        errChiTiet.SetError(txtMaSP, "Mã sản phẩm trùng trong cơ sở dữ liệu");
                        return;
                    }
                    errChiTiet.Clear();
                }
                //Insert vao CSDL
                sql = "INSERT INTO tblGear(masp,tensp,soluong,gia,thongso)VALUES (";
                sql += "N'" + txtMaSP.Text + "',N'" + txtTenSP.Text + "','" + txtSoLuong.Text + "',N'" + txtGia.Text + "',N'" + txtThongSo.Text + "')";
            }
            //Sua sp
            if (btnSua.Enabled == true)
            {
                sql = "Update tblGear SET ";
                sql += "tensp = N'" + txtTenSP.Text + "',";
                sql += "soluong = N'" + txtSoLuong.Text + "',";
                sql += "gia = '" + txtGia.Text + "',";
                sql += "thongso = N'" + txtThongSo.Text + "' ";
                sql += "Where masp = N'" + txtMaSP.Text + "'";
            }
            //Xoa sp
            if (btnXoa.Enabled == true)
            {
                sql = "Delete From tblGear Where masp =N'" + txtMaSP.Text + "'";
            }
            //Thuc thi cau lenh sql
            cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            //Cap nhat lai DataGrid
            sql = "Select * from tblGear";
            LoadDuLieu(sql);
            //dong ket noi
            con.Close();
            //an hien cac nut chuc nang
            HienChiTiet(false);
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }
        //Click nut huy
        private void btnHuy_Click(object sender, EventArgs e)
        {
            //thiet lap cac nut nhu ban dau
            btnXoa.Enabled = false;
            btnSua.Enabled = false;
            btnThem.Enabled = true;
            //xoa trang
            XoaTrangChiTiet();
            //Cap nhat
            HienChiTiet(false);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            //dong form
            this.Close();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtMaSP_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void lblTieuDe_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtTKMaSP_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
