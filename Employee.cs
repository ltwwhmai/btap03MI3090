/****************/
// 202418940
// Lê Thanh Mai
/****************/
using System;

/// <summary>
/// Lớp cơ sở đại diện cho nhân viên thông thường.
/// Kế thừa IDisposable để cung cấp cơ chế giải phóng tài nguyên khi không còn sử dụng.
/// </summary>
class Employee : IDisposable
{
    // Các trường dữ liệu bảo vệ (protected) cho phép lớp con (kế thừa) có thể truy cập trực tiếp,
    // nhưng bị ẩn đi đối với bên ngoài (hỗ trợ tính đóng gói).
    protected string ma, ho_ten;
    protected double luong_co_ban;

    /// <summary>
    /// Constructor mặc định. Khởi tạo nhân viên vô danh.
    /// Sử dụng từ khóa 'this' để gọi sang constructor 3 tham số, tránh lặp code.
    /// </summary>
    public Employee() : this("UNKNOWN", "Unnamed employee", 0) { }

    /// <summary>
    /// Constructor 2 tham số (chỉ có mã và tên). Mặc định lương bằng 0.
    /// </summary>
    public Employee(string ma, string ho_ten) : this(ma, ho_ten, 0) { }

    /// <summary>
    /// Constructor đầy đủ tham số. Đây là nơi duy nhất thực hiện việc gán và kiểm tra dữ liệu đầu vào.
    /// </summary>
    public Employee(string ma, string ho_ten, double luong_co_ban)
    {
        // Kiểm tra tính hợp lệ của chuỗi: không được null, rỗng hoặc chỉ chứa khoảng trắng
        if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ho_ten))
            throw new ArgumentException("Mã và họ tên không được rỗng");

        // Kiểm tra tính hợp lệ của lương: không được là số âm
        if (luong_co_ban < 0)
            throw new ArgumentException("Lương cơ bản không được âm");

        // Gán dữ liệu hợp lệ vào các thuộc tính của lớp
        this.ma = ma;
        this.ho_ten = ho_ten;
        this.luong_co_ban = luong_co_ban;
    }

    // Các properties (thuộc tính) chỉ đọc (get-only) sử dụng cú pháp Expression-bodied (=>)
    // Giúp bên ngoài có thể lấy thông tin nhưng không thể tự ý sửa đổi
    public string Id => ma;
    public string FullName => ho_ten;
    public double BaseSalary => luong_co_ban;

    /// <summary>
    /// Nạp chồng phương thức (Overloading): Tăng lương theo một số tiền cố định.
    /// Gọi lại hàm IncreaseSalary với cờ theo_phan_tram = false.
    /// </summary>
    public void IncreaseSalary(double so_tien) => IncreaseSalary(so_tien, false);

    /// <summary>
    /// Nạp chồng phương thức (Overloading): Tăng lương linh hoạt theo số tiền cố định hoặc theo phần trăm.
    /// </summary>
    /// <param name="gia_tri">Số tiền hoặc số phần trăm muốn tăng</param>
    /// <param name="theo_phan_tram">True nếu gia_tri là phần trăm, False nếu là số tiền</param>
    public void IncreaseSalary(double gia_tri, bool theo_phan_tram)
    {
        // Ràng buộc giá trị tăng phải lớn hơn 0
        if (gia_tri <= 0) throw new ArgumentException("Giá trị tăng phải dương");

        // Toán tử 3 ngôi: Nếu theo_phan_tram là true -> tính % của lương cơ bản. Nếu sai -> cộng thẳng giá_tri.
        luong_co_ban += theo_phan_tram ? luong_co_ban * gia_tri / 100 : gia_tri;
    }

    /// <summary>
    /// Tính chi phí hàng tháng của nhân viên.
    /// Từ khóa 'virtual' cho phép các lớp con (như Manager, Engineer...) ghi đè (override) lại công thức tính này.
    /// </summary>
    public virtual double CalculateMonthlyCost() => luong_co_ban;

    /// <summary>
    /// In thông tin nhân viên ra màn hình.
    /// Sử dụng String Interpolation ($"") để chèn biến và định dạng số (:N0) để thêm dấu phẩy ngăn cách hàng nghìn.
    /// Có thể bị ghi đè ở lớp con nhờ từ khóa 'virtual'.
    /// </summary>
    public virtual void DisplayInfo() =>
        Console.WriteLine($"[Employee] {ma} | {ho_ten} | Lương: {luong_co_ban:N0} | Chi phí: {CalculateMonthlyCost():N0}");

    /// <summary>
    /// Triển khai phương thức Dispose của giao diện IDisposable.
    /// Dùng để dọn dẹp hoặc in thông báo khi đối tượng bị thu hồi.
    /// Từ khóa 'virtual' cho phép lớp con bổ sung thêm logic dọn dẹp riêng của nó.
    /// </summary>
    public virtual void Dispose() => Console.WriteLine($"  (Hủy Employee {ma})");
}