USE NhaHang;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN ('Nam', 'N?', 'Khác')),
    SDT VARCHAR(15) UNIQUE NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    MatKhau NVARCHAR(255) NOT NULL, -- L?u m?t kh?u sau khi mã hóa
    DiaChi NVARCHAR(255),
    VaiTro NVARCHAR(50) CHECK (VaiTro IN ('KhachHang', 'NhanVienPhucVu', 'NhanVienBep', 'QuanLy')) NOT NULL
);


SELECT name, definition 
FROM sys.check_constraints 
WHERE parent_object_id = OBJECT_ID('Users') AND name = 'CK__Users__GioiTinh__3F466844';

UPDATE Users SET GioiTinh = 'Nam' WHERE GioiTinh NOT IN ('Nam', 'N?', 'Khác');
