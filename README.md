Known Issues

Fitur Cetak Laporan (Crystal Report) Belum Berfungsi Sempurna

Form3 (Cetak Laporan) saat ini menampilkan halaman report yang kosong/tidak ter-binding dengan benar pada beberapa field data (Nama, Jenis Kelamin, Alamat, Nama Prodi, Tanggal Daftar, dan Total Mahasiswa).

Root cause: Field pada file ReportMahasiswa.rpt tidak ter-binding dengan benar ke Database Fields (ClassReport) yang sudah didefinisikan, sehingga Crystal Report Designer menampilkan nama field itu sendiri alih-alih nilai data yang sebenarnya. Saat dicoba diperbaiki langsung melalui Crystal Report Designer (drag-drop ulang field dari Field Explorer), proses tersebut memicu error tambahan (The remaining text does not appear to be part of the formula) serta menyebabkan file .rpt membuat ulang file class duplikat (ReportMahasiswa1.cs) yang sempat menimbulkan conflict compile (CS0101/CS0111/CS0121).

Status: Untuk sementara, fitur ini di-nonaktifkan/di-skip agar tidak mengganggu stabilitas build aplikasi secara keseluruhan. Seluruh fitur lain (Dashboard, CRUD Mahasiswa, Upload Foto, Import Excel, dan Rekap Data tanpa cetak) telah diuji dan berfungsi dengan baik.

Rencana perbaikan: Field pada .rpt perlu di-rebuild dari awal melalui Crystal Report Designer dengan melakukan binding ulang seluruh field secara hati-hati satu per satu langsung dari Database Fields, tanpa melalui proses edit formula yang berisiko memicu file duplikat.
