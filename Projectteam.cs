/****************/
// 202418940
// Lê Thanh Mai
/****************/
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Lớp đại diện cho một nhóm dự án.
/// Quan hệ giữa ProjectTeam và Employee là Kết tập (Aggregation): 
/// Nhóm dự án quản lý danh sách nhân sự nhưng không sở hữu sự tồn tại của nhân sự đó.
/// Khi hủy nhóm, các đối tượng Employee thực tế ở bên ngoài vẫn tiếp tục tồn tại.
/// </summary>
class ProjectTeam : IDisposable
{
    // Các trường lưu trữ thông tin cơ bản của dự án
    string ma_du_an, ten_du_an;

    // Tham chiếu đến đối tượng nhân sự giữ vai trò trưởng nhóm
    Employee truong_nhom;

    // Danh sách liên kết lưu trữ các thành viên tham gia nhóm
    List<Employee> thanh_vien = new List<Employee>();

    /// <summary>
    /// Constructor 2 tham số: Khởi tạo thông tin nhóm dự án cơ bản (chưa có thành viên).
    /// </summary>
    public ProjectTeam(string ma_du_an, string ten_du_an)
    {
        // Kiểm tra tính hợp lệ: mã và tên không được để trống
        if (string.IsNullOrWhiteSpace(ma_du_an) || string.IsNullOrWhiteSpace(ten_du_an))
            throw new ArgumentException("Mã và tên dự án không được rỗng");

        this.ma_du_an = ma_du_an;
        this.ten_du_an = ten_du_an;
    }

    /// <summary>
    /// Constructor 3 tham số: Khởi tạo nhóm và chỉ định ngay một trưởng nhóm ban đầu.
    /// Cú pháp ': this(...)' giúp gọi lại constructor 2 tham số ở trên để tái sử dụng logic kiểm tra lỗi.
    /// </summary>
    public ProjectTeam(string ma_du_an, string ten_du_an, Employee truong_nhom) : this(ma_du_an, ten_du_an)
    {
        // Đảm bảo đối tượng truyền vào không bị null
        if (truong_nhom == null) throw new ArgumentNullException(nameof(truong_nhom));

        // Gọi hàm thêm thành viên và gắn cờ true để cấp quyền trưởng nhóm
        AddMember(truong_nhom, true);
    }

    /// <summary>
    /// Kiểm tra xem một nhân sự có mã tương ứng đã tồn tại trong nhóm chưa.
    /// Sử dụng phương thức Any() của LINQ để duyệt nhanh danh sách.
    /// </summary>
    public bool Contains(string ma) => thanh_vien.Any(n => n.Id == ma);

    /// <summary>
    /// Nạp chồng (Overloading): Thêm một thành viên vào nhóm với tư cách thành viên thường.
    /// </summary>
    public bool AddMember(Employee nhan_su) => AddMember(nhan_su, false);

    /// <summary>
    /// Nạp chồng (Overloading): Thêm thành viên, kèm theo tùy chọn có đặt làm trưởng nhóm hay không.
    /// </summary>
    /// <param name="nhan_su">Đối tượng nhân sự cần thêm</param>
    /// <param name="lam_truong">True nếu chỉ định người này làm trưởng nhóm</param>
    public bool AddMember(Employee nhan_su, bool lam_truong)
    {
        // Chặn luồng nếu dữ liệu truyền vào là null HOẶC người này đã có trong danh sách (tránh thêm trùng lặp)
        if (nhan_su == null || Contains(nhan_su.Id)) return false;

        // Thêm vào danh sách nội bộ
        thanh_vien.Add(nhan_su);

        // Cập nhật tham chiếu trưởng nhóm nếu cờ lam_truong được bật
        if (lam_truong) truong_nhom = nhan_su;
        return true;
    }

    /// <summary>
    /// Xóa một thành viên khỏi nhóm dựa theo mã ID.
    /// </summary>
    public bool RemoveMember(string ma)
    {
        // LINQ FirstOrDefault: Tìm người đầu tiên khớp mã, nếu không tìm thấy thì trả về null
        var nguoi = thanh_vien.FirstOrDefault(n => n.Id == ma);

        // Ràng buộc: Không thể xóa nếu không tìm thấy người đó, 
        // HOẶC nếu người đó đang giữ chức trưởng nhóm (phải đổi trưởng nhóm khác trước khi xóa).
        if (nguoi == null || nguoi == truong_nhom) return false;

        // Xóa thành công khỏi danh sách
        return thanh_vien.Remove(nguoi);
    }

    /// <summary>
    /// Bổ nhiệm một nhân sự khác làm trưởng nhóm mới.
    /// </summary>
    public bool ChangeLeader(Employee nhan_su)
    {
        if (nhan_su == null) return false;

        // Cố gắng thêm người này vào danh sách. 
        // Nếu người này đã có sẵn trong danh sách thì hàm AddMember sẽ trả về false và bỏ qua bước thêm mới.
        AddMember(nhan_su);

        // LINQ First: Tìm lại chính xác tham chiếu của người đó trong danh sách để gán vào biến truong_nhom
        truong_nhom = thanh_vien.First(n => n.Id == nhan_su.Id);
        return true;
    }

    /// <summary>
    /// Tính tổng chi phí lương và phụ cấp của toàn bộ dự án.
    /// Sử dụng LINQ Sum kết hợp với tính Đa hình (phương thức CalculateMonthlyCost sẽ tự động tính đúng dựa theo việc phần tử là Employee hay SoftwareEngineer).
    /// </summary>
    public double CalculateTotalMonthlyCost() => thanh_vien.Sum(n => n.CalculateMonthlyCost());

    /// <summary>
    /// In chi tiết thông tin của dự án và các thành viên bên trong.
    /// </summary>
    public void DisplayTeam()
    {
        // Toán tử '?.Id' an toàn lấy thuộc tính Id nếu truong_nhom có tồn tại.
        // Toán tử '??' sẽ cung cấp giá trị mặc định là "chưa có" nếu truong_nhom đang bị null.
        Console.WriteLine($"--- Nhóm {ma_du_an}: {ten_du_an} | Trưởng nhóm: {truong_nhom?.Id ?? "chưa có"} ---");

        // Vòng lặp foreach gọi phương thức in thông tin của từng đối tượng.
        // Đây là minh chứng của Đa hình (Polymorphism) qua hàm DisplayInfo() đã được khai báo 'virtual' và 'override'.
        foreach (var n in thanh_vien) n.DisplayInfo();
    }

    /// <summary>
    /// Triển khai cơ chế giải phóng bộ nhớ từ giao diện IDisposable.
    /// Do quan hệ là Kết tập, ở đây ta chỉ cắt đứt các tham chiếu (Clear danh sách, gán null trưởng nhóm),
    /// tuyệt đối KHÔNG được hủy (Dispose) các đối tượng Employee bên trong.
    /// </summary>
    public void Dispose()
    {
        thanh_vien.Clear();
        truong_nhom = null;
        Console.WriteLine($"  (Hủy ProjectTeam {ma_du_an})");
    }
}