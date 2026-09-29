/****************/
// 202418940
// Lê Thanh Mai
/****************/
using System;

/// <summary>
/// Lớp đại diện cho Kỹ sư phần mềm.
/// Sử dụng cú pháp ': Employee' để Kế thừa (Inherit) toàn bộ thuộc tính và phương thức từ lớp cha Employee.
/// </summary>
class SoftwareEngineer : Employee
{
    // Các trường dữ liệu bổ sung, đặc thù riêng của kỹ sư phần mềm
    string ngon_ngu_chinh;
    double phu_cap;

    /// <summary>
    /// Constructor 3 tham số (mã, tên, ngôn ngữ chính).
    /// Cú pháp ': this(...)' gọi lại constructor 5 tham số của chính lớp này, truyền giá trị 0 cho lương và phụ cấp.
    /// </summary>
    public SoftwareEngineer(string ma, string ho_ten, string ngon_ngu_chinh)
        : this(ma, ho_ten, 0, ngon_ngu_chinh, 0) { }

    /// <summary>
    /// Constructor 5 tham số đầy đủ.
    /// Cú pháp ': base(ma, ho_ten, luong_co_ban)' sẽ đẩy 3 tham số cơ bản lên cho Constructor của lớp cha (Employee) xử lý,
    /// lớp con (SoftwareEngineer) chỉ cần tập trung xử lý và kiểm tra các tham số riêng của nó.
    /// </summary>
    public SoftwareEngineer(string ma, string ho_ten, double luong_co_ban, string ngon_ngu_chinh, double phu_cap)
        : base(ma, ho_ten, luong_co_ban)
    {
        // Kiểm tra tính hợp lệ của dữ liệu đầu vào đặc thù của kỹ sư
        if (string.IsNullOrWhiteSpace(ngon_ngu_chinh))
            throw new ArgumentException("Ngôn ngữ chính không được rỗng");

        if (phu_cap < 0)
            throw new ArgumentException("Phụ cấp không được âm");

        this.ngon_ngu_chinh = ngon_ngu_chinh;
        this.phu_cap = phu_cap;
    }

    /// <summary>
    /// Ghi đè (Override) phương thức tính chi phí từ lớp cha.
    /// Thể hiện tính Đa hình: Với kỹ sư, chi phí hàng tháng sẽ cộng thêm phần 'phụ cấp' thay vì chỉ có lương cơ bản.
    /// Thuộc tính 'luong_co_ban' lấy trực tiếp từ lớp cha do được khai báo là 'protected'.
    /// </summary>
    public override double CalculateMonthlyCost() => luong_co_ban + phu_cap;

    /// <summary>
    /// Ghi đè phương thức hiển thị thông tin.
    /// Định dạng lại chuỗi đầu ra để bổ sung thêm Ngôn ngữ và Phụ cấp.
    /// Lời gọi hàm CalculateMonthlyCost() ở đây sẽ tự động chạy vào hàm đã bị ghi đè ở ngay phía trên.
    /// </summary>
    public override void DisplayInfo() =>
        Console.WriteLine($"[Engineer] {ma} | {ho_ten} | Ngôn ngữ: {ngon_ngu_chinh} | Lương: {luong_co_ban:N0} | Phụ cấp: {phu_cap:N0} | Chi phí: {CalculateMonthlyCost():N0}");

    /// <summary>
    /// Ghi đè phương thức dọn dẹp bộ nhớ.
    /// </summary>
    public override void Dispose()
    {
        // In ra thông báo dọn dẹp riêng của lớp con
        Console.WriteLine($"  (Hủy SoftwareEngineer {ma})");

        // Từ khóa 'base' dùng để gọi lại logic dọn dẹp nguyên thủy của lớp cha,
        // đảm bảo toàn bộ tài nguyên (nếu có) từ cả lớp cha và con đều được giải phóng hoàn toàn.
        base.Dispose();
    }
}