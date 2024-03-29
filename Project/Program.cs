﻿using System.Data.Common;
using System.Text;
using NKalkan;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Diagnostics.CodeAnalysis;

var certificatePath = "./GOST512_8147287582df9bb4710e461804acd49b88bf45c4.p12";
var certificatePassword = "Qwerty12";
var documentToSign = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus leo felis, pretium sed pellentesque at, suscipit eu nibh. Phasellus gravida odio elit, vitae lacinia nisi. Etiam volutpat, nisl et vulputate vulputate, ipsum erat consectetur ante, a ultricies nunc elit semper neque.";
var data = Encoding.UTF8.GetBytes(documentToSign);
var client = new KalkanApi();
// client.LoadKeyStore(KalkanStorageType.PKCS12, certificatePath, certificatePassword);

// var s = @"-----BEGIN CMS-----
// MIIPrAYJKoZIhvcNAQcCoIIPnTCCD5kCAQExDjAMBggqgw4DCgEDAwUAMAsGCSqG
// SIb3DQEHAaCCBDowggQ2MIIDnqADAgECAhQRRyh1gt+btHEORhgErNSbiL9FxDAO
// Bgoqgw4DCgEBAgMCBQAwXTFOMEwGA1UEAwxF0rDQm9Ci0KLQq9KaINCa0KPTmNCb
// 0JDQndCU0KvQoNCj0KjQqyDQntCg0KLQkNCb0KvSmiAoR09TVCkgVEVTVCAyMDIy
// MQswCQYDVQQGEwJLWjAeFw0yMzExMDkxMDE4NDBaFw0yNDExMDgxMDE4NDBaMHkx
// HjAcBgNVBAMMFdCi0JXQodCi0J7QkiDQotCV0KHQojEVMBMGA1UEBAwM0KLQldCh
// 0KLQntCSMRgwFgYDVQQFEw9JSU4xMjM0NTY3ODkwMTExCzAJBgNVBAYTAktaMRkw
// FwYDVQQqDBDQotCV0KHQotCe0JLQmNCnMIGsMCMGCSqDDgMKAQECAjAWBgoqgw4D
// CgEBAgIBBggqgw4DCgEDAwOBhAAEgYDa/NKfEL8rvhXRv1DMn+vaYz0bGFs6ixgo
// jRIEKcCjYht4DkcrPOGW3k+ER4YR1M3jCv1tb7FHi/EQFWoOeIBhFHq6cJ/M6ZHL
// ucyjnIDgk/C7zvbg5mXB7YIQGyYHK0DJF4K2HpFkzJ4DNMP9LprKgGYp9UCUIUH0
// FflwQlVXaqOCAcYwggHCMDgGA1UdIAQxMC8wLQYGKoMOAwMCMCMwIQYIKwYBBQUH
// AgEWFWh0dHA6Ly9wa2kuZ292Lmt6L2NwczB3BggrBgEFBQcBAQRrMGkwKAYIKwYB
// BQUHMAGGHGh0dHA6Ly90ZXN0LnBraS5nb3Yua3ovb2NzcC8wPQYIKwYBBQUHMAKG
// MWh0dHA6Ly90ZXN0LnBraS5nb3Yua3ovY2VydC9uY2FfZ29zdDIwMjJfdGVzdC5j
// ZXIwQQYDVR0fBDowODA2oDSgMoYwaHR0cDovL3Rlc3QucGtpLmdvdi5rei9jcmwv
// bmNhX2dvc3QyMDIyX3Rlc3QuY3JsMEMGA1UdLgQ8MDowOKA2oDSGMmh0dHA6Ly90
// ZXN0LnBraS5nb3Yua3ovY3JsL25jYV9nb3N0MjAyMl9kX3Rlc3QuY3JsMB0GA1Ud
// JQQWMBQGCCsGAQUFBwMEBggqgw4DAwQBATAOBgNVHQ8BAf8EBAMCA8gwHQYDVR0O
// BBYEFIFHKHWC35u0cQ5GGASs1JuIv0XEMB8GA1UdIwQYMBaAFPrSSxujoMlh/hyo
// UD5qortFDbijMBYGBiqDDgMDBQQMMAoGCCqDDgMDBQEBMA4GCiqDDgMKAQECAwIF
// AAOBgQBpz+3kpvElKZfsyHVbOWqbzdS5jqIafZOucNNM3SfqgW40FP2UXK9fofDB
// cXsrZxXQL8P9t3a+9OstVN2KV3rKpf7St/iYe0t9kCjZZi370t7JtamkTZkaRrFc
// JLZ2L5tnDI+hXY2IDRAlGBAC24IPLstj6nJIE1S28F1ReBhzEzGCCzcwggszAgEB
// MHUwXTFOMEwGA1UEAwxF0rDQm9Ci0KLQq9KaINCa0KPTmNCb0JDQndCU0KvQoNCj
// 0KjQqyDQntCg0KLQkNCb0KvSmiAoR09TVCkgVEVTVCAyMDIyMQswCQYDVQQGEwJL
// WgIUEUcodYLfm7RxDkYYBKzUm4i/RcQwDAYIKoMOAwoBAwMFAKCBwjAYBgkqhkiG
// 9w0BCQMxCwYJKoZIhvcNAQcBMBwGCSqGSIb3DQEJBTEPFw0yMzEyMjcxNTI0NDRa
// MDcGCyqGSIb3DQEJEAIvMSgwJjAkMCIEIBu6B9ZqO4PyIjmdoiU5ELt4u2GOUY5a
// KzbbJgQh2nOTME8GCSqGSIb3DQEJBDFCBEA6JBbKqIliWb54+OwvbgAaqmTfrMfY
// OUeMVZ2KQb7sznIwVZSrGNb10WymqzkDnYVoM5TrXKcr+GqMAtp0ltYHMA4GCiqD
// DgMKAQECAwIFAASBgHwruezUvDrLlswO02aFmtHuUm5vPJt7zDagSmJ6nvpC1/C4
// R7mdLM9nOgnPo8LTj3qjBRDQxI3ZO7srzSG4WS7a79Kj9RmPoe4NXU2GNpd01Ocz
// msC55Flw6SehY8eGu/TFVMc25wUiAuGxBHYoybbnbhyX9G5J04nAEsV/F084oYIJ
// TzCCCUsGCyqGSIb3DQEJEAIOMYIJOjCCCTYGCSqGSIb3DQEHAqCCCScwggkjAgED
// MQ8wDQYJYIZIAWUDBAIBBQAwgYQGCyqGSIb3DQEJEAEEoHUEczBxAgEBBggqgw4D
// AwIGAjAxMA0GCWCGSAFlAwQCAQUABCB93hJhT7l359H7QzIXFHUlZOtkULNxr+yC
// hOTlcHujGgIU2daLL+HLWv+r5wK7hIDsp73hZJQYDzIwMjMxMjI3MTUyNDQ0WgII
// KbLiQgNdtgGgggZQMIIGTDCCBDSgAwIBAgIUEQEyiRePazp8Z4cWI+7yANvw914w
// DQYJKoZIhvcNAQELBQAwUjELMAkGA1UEBhMCS1oxQzBBBgNVBAMMOtKw0JvQotCi
// 0KvSmiDQmtCj05jQm9CQ0J3QlNCr0KDQo9Co0Ksg0J7QoNCi0JDQm9Cr0pogKFJT
// QSkwHhcNMjIxMDE4MDU0MTI4WhcNMjUwNjAxMDU0MTI4WjCCAQQxFDASBgNVBAMM
// C1RTQSBTRVJWSUNFMRgwFgYDVQQFEw9JSU43NjEyMzEzMDAzMTMxCzAJBgNVBAYT
// AktaMRUwEwYDVQQHDAzQkNCh0KLQkNCd0JAxFTATBgNVBAgMDNCQ0KHQotCQ0J3Q
// kDEYMBYGA1UECwwPQklOMDAwNzQwMDAwNzI4MX0wewYDVQQKDHTQkNCa0KbQmNCe
// 0J3QldCg0J3QntCVINCe0JHQqdCV0KHQotCS0J4gItCd0JDQptCY0J7QndCQ0JvQ
// rNCd0KvQlSDQmNCd0KTQntCg0JzQkNCm0JjQntCd0J3Qq9CVINCi0JXQpdCd0J7Q
// m9Ce0JPQmNCYIjCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAJlB802e
// QhUW5sqEcMUtP+fao9YzyIEMuJn2yzgB9ZtGbztsMkiPjmr/+AYNaYmy9QZJn84P
// rE9GZ65rZNuFLlUceje820EG+9XtVhK2w5zMMT8vwg0TmIKProVTBiuZHM1EJ4pF
// sRdxYlJ54Njdr3FzFJOst3nFBEKQP7yCExE0XO2tOYaXmgunwXb4fDLME/R5l1u7
// HqYIxwn8j5WDkzN/zNy/biy4NdqrWeijB7aTGowd39aadkIjg7pEWXHJLr4dxJ1m
// X7DlyuOQ6LLDnU/aByEcvmSoKriGbnfW5pp7dIq6y/pxJV9Yv7eSUvODuhyca5Oy
// FB3CUELLkvdsGn8CAwEAAaOCAWQwggFgMBYGA1UdJQEB/wQMMAoGCCsGAQUFBwMI
// MA8GA1UdIwQIMAaABFtqdBEwHQYDVR0OBBYEFLQmnhxQAFDx60w+Fiqpu7UOEca6
// MFYGA1UdHwRPME0wS6BJoEeGIWh0dHA6Ly9jcmwucGtpLmdvdi5rei9uY2FfcnNh
// LmNybIYiaHR0cDovL2NybDEucGtpLmdvdi5rei9uY2FfcnNhLmNybDBaBgNVHS4E
// UzBRME+gTaBLhiNodHRwOi8vY3JsLnBraS5nb3Yua3ovbmNhX2RfcnNhLmNybIYk
// aHR0cDovL2NybDEucGtpLmdvdi5rei9uY2FfZF9yc2EuY3JsMGIGCCsGAQUFBwEB
// BFYwVDAuBggrBgEFBQcwAoYiaHR0cDovL3BraS5nb3Yua3ovY2VydC9uY2FfcnNh
// LmNlcjAiBggrBgEFBQcwAYYWaHR0cDovL29jc3AucGtpLmdvdi5rejANBgkqhkiG
// 9w0BAQsFAAOCAgEAEvCHejpnmVLCebGHXEI3oOLVYCCo0Q0fjHBCSa+a79qnTXM3
// YZEBMYsxr909WY2FMU3zEjUWmA0d2zseCmo+DnKo9lmESqFQi6g3uFYnRrJ7aEi/
// J1IHN04PHOc1gO1Dgt82+wB08n5+lMjxaejVIZhunJwMJX+78obd/R/be/MFFeR5
// 0JAkG3D+2ieRvkYJ0AqU+1L8n/hY1R8z3aPQkKdP6ClYuDOTQOIlW+P2xL0PicYQ
// IwdJF2qxZD/52Xg4JM0a6TYxU/01HXAQC7QOU9AqFW54de6j5CyTXfmd8XRLVP6h
// ecsupcH3mR/9PM4VNhT+KicgxoWE3cBiUEg5usOa9Si0wN5+ecUqI8NVTm9VcH5N
// R7jtFg/77FIHDb7Pjm6JqNQQZLKPd+SnnzDVRq149rE1JlX8YCIElhcmosuLhLNW
// BN8MLX6m6QHRTcWa0jGzsQ9O4dkVF6evmxvmvG7M39TCLJ1igSCk4Y6lDFLaQyd8
// Nc6TUwCY1Pjt5jXwadcD/srFCRdVrk6GsDy9bLvcoK8/m0Rbkz1DWVFvnBqleECM
// u1d0HkZBHykdAudSQZSDIUZtRt6NV2gpYTJDXjx+relHxOna7TFLFxe+c2jv1jBu
// B0zGfEPXV5YJQO8T1ofLRDjyyVdanXNnLroPbsE3GjW3zc/dXguRSIrVtCcxggIw
// MIICLAIBATBqMFIxCzAJBgNVBAYTAktaMUMwQQYDVQQDDDrSsNCb0KLQotCr0pog
// 0JrQo9OY0JvQkNCd0JTQq9Cg0KPQqNCrINCe0KDQotCQ0JvQq9KaIChSU0EpAhQR
// ATKJF49rOnxnhxYj7vIA2/D3XjANBglghkgBZQMEAgEFAKCBmDAaBgkqhkiG9w0B
// CQMxDQYLKoZIhvcNAQkQAQQwHAYJKoZIhvcNAQkFMQ8XDTIzMTIyNzE1MjQ0NFow
// KwYLKoZIhvcNAQkQAgwxHDAaMBgwFgQUt2MG9k4ovM6ZyFKVl67mlskKjoAwLwYJ
// KoZIhvcNAQkEMSIEIKZvGVo60DhktUUWPNNGCz5TLFD8UUdq01iNpD/76WXDMA0G
// CSqGSIb3DQEBCwUABIIBAFc62hHd/gQvejkrXOLzESehD2c9azd/LzXiE5lkTKDP
// wnUV/dG8TbtzL80j0c/yX7yAIqTKuP3i1HoqJleLFGCsZKe9P96u7z/jYTxkwKRC
// dBzPAZRVOBDHlV7zaxPP0F8f0wZwiLDqqy05GIA578eh6N1OH3/nCWr3GwKjruRJ
// A8aucx1392XjxPgBdQhaLULGegj3mXTi2qbUvCeVIkUiW2/P0Sn8prvLnwvChXwC
// AYrWV+IcgKVThQBLeWTkrsHE8fe4pplnxduyBpWb1JpqUhokw1z4kpFYQKBP7CRg
// Mqx8igatzLErKFYw7p7BKFAwuXq62nejcqKNkJsYBek=
// -----END CMS-----";

var xmlb64 = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiIHN0YW5kYWxvbmU9Im5vIj8+PFRlbmRlclNpZ25EYXRhPgogIDxUZW5kZXJOdW1iZXI+VC0wMDAwMDEyPC9UZW5kZXJOdW1iZXI+CiAgPE9yZ2FuaXplckluZm8+bmNhbGF5ZXIgY29tcGFueTI8L09yZ2FuaXplckluZm8+CiAgPENvbmZpZ05hbWU+0JDRg9C60YbQuNC+0L0g0L3QsCDQv9C+0L3QuNC20LXQvdC40LU8L0NvbmZpZ05hbWU+CiAgPFRlbmRlckRvY3VtZW50cy8+CiAgPFRlbmRlckNvbnRhY3RQZXJzb25zPgogICAgPENvbnRhY3RQZXJzb24geG1sbnM6eHNkPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSI+CiAgICAgIDxQZXJzb25VaWQ+OThjODJhMzktZTlhYi00NWNmLTg2ZTEtMWU4ZjJkMDcwZDRiPC9QZXJzb25VaWQ+CiAgICAgIDxGdWxsTmFtZT7QotCV0KHQotCe0JIg0KLQldCh0KLQntCS0JjQpzwvRnVsbE5hbWU+CiAgICAgIDxFbWFpbD5uY2FsYXllcjJAZXhhbXBsZS5jb208L0VtYWlsPgogICAgICA8UGhvbmU+MzQzMzI0MzQyMzQyMzM8L1Bob25lPgogICAgPC9Db250YWN0UGVyc29uPgogIDwvVGVuZGVyQ29udGFjdFBlcnNvbnM+CiAgPExvdHM+CiAgICA8TG90PgogICAgICA8TG90TnVtYmVyRnVsbD5ULTAwMDAwMTIvMTwvTG90TnVtYmVyRnVsbD4KICAgICAgPExvdERvY3VtZW50cy8+CiAgICAgIDxMb3RJdGVtcy8+CiAgICAgIDxMb3RGaWVsZHM+CiAgICAgICAgPFRpdGxlPtCb0L7RgiDihJYxPC9UaXRsZT4KICAgICAgICA8UHJpY2UuVmFsdWUvPgogICAgICAgIDxQcmljZUN1cnJlbmN5PtCi0LXQvdCz0LU8L1ByaWNlQ3VycmVuY3k+CiAgICAgICAgPFByaWNlU3RlcD4gPC9QcmljZVN0ZXA+CiAgICAgICAgPFdpbm5lclNlbGVjdGlvbkxldmVsPtCf0L7QsdC10LTQuNGC0LXQu9GMINCy0YvQsdC40YDQsNC10YLRgdGPINC/0L4g0LvQvtGC0YM8L1dpbm5lclNlbGVjdGlvbkxldmVsPgogICAgICAgIDxLYXRvLz4KICAgICAgICA8Tm90ZXMvPgogICAgICA8L0xvdEZpZWxkcz4KICAgIDwvTG90PgogIDwvTG90cz4KICA8VGVuZGVyRmllbGRzPgogICAgPFRpdGxlPtCQ0YPQutGG0LjQvtC9INCy0L3QuNC3PC9UaXRsZT4KICAgIDxSZXF1ZXN0UmVjZWl2aW5nQmVnaW5EYXRlVHlwZT7Qo9C60LDQt9Cw0L3QvdCw0Y8g0LTQsNGC0LA8L1JlcXVlc3RSZWNlaXZpbmdCZWdpbkRhdGVUeXBlPgogICAgPFJlcXVlc3RSZWNlaXZpbmdCZWdpbkRhdGUvPgogICAgPFJlcXVlc3RSZWNlaXZpbmdFbmREYXRlVHlwZT7Qo9C60LDQt9Cw0L3QvdCw0Y8g0LTQsNGC0LA8L1JlcXVlc3RSZWNlaXZpbmdFbmREYXRlVHlwZT4KICAgIDxSZXF1ZXN0UmVjZWl2aW5nRW5kRGF0ZS8+CiAgICA8QmlkUmVjZWl2aW5nQmVnaW5EYXRlLz4KICAgIDxQcmljZUN1cnJlbmN5PtCi0LXQvdCz0LU8L1ByaWNlQ3VycmVuY3k+CiAgICA8UGFydGljaXBhdGlvbkZvcm0+0J7RgtC60YDRi9GC0LDRjzwvUGFydGljaXBhdGlvbkZvcm0+CiAgICA8VmlzaWJpbGl0eUZvcm0+0J7RgtC60YDRi9GC0LDRjzwvVmlzaWJpbGl0eUZvcm0+CiAgICA8UGFydGljaXBhdGlvblJlcXVlc3RGb3JtPtCe0YLQutGA0YvRgtCw0Y88L1BhcnRpY2lwYXRpb25SZXF1ZXN0Rm9ybT4KICAgIDxFY29ub21pY0FjdGl2aXR5Lz4KICAgIDxOb3Rlcy8+CiAgPC9UZW5kZXJGaWVsZHM+CjxkczpTaWduYXR1cmUgeG1sbnM6ZHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPgo8ZHM6U2lnbmVkSW5mbz4KPGRzOkNhbm9uaWNhbGl6YXRpb25NZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy9UUi8yMDAxL1JFQy14bWwtYzE0bi0yMDAxMDMxNSIvPgo8ZHM6U2lnbmF0dXJlTWV0aG9kIEFsZ29yaXRobT0idXJuOmlldGY6cGFyYW1zOnhtbDpuczpwa2lnb3Zrejp4bWxzZWM6YWxnb3JpdGhtczpnb3N0cjM0MTAyMDE1LWdvc3RyMzQxMTIwMTUtNTEyIi8+CjxkczpSZWZlcmVuY2UgVVJJPSIiPgo8ZHM6VHJhbnNmb3Jtcz4KPGRzOlRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNlbnZlbG9wZWQtc2lnbmF0dXJlIi8+CjxkczpUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy9UUi8yMDAxL1JFQy14bWwtYzE0bi0yMDAxMDMxNSNXaXRoQ29tbWVudHMiLz4KPC9kczpUcmFuc2Zvcm1zPgo8ZHM6RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0idXJuOmlldGY6cGFyYW1zOnhtbDpuczpwa2lnb3Zrejp4bWxzZWM6YWxnb3JpdGhtczpnb3N0cjM0MTEyMDE1LTUxMiIvPgo8ZHM6RGlnZXN0VmFsdWU+UTdoWk5ZMUtDSGpYZE4rUTIxT1Y0dDNrcXhhbTBrTm9WWDFFN2N3UFY1U0duQTR3QjJERVB0Uk92cnB5R0FoK0txSGxWNUp0UGJtTgo0QzM4T0o3eDl3PT08L2RzOkRpZ2VzdFZhbHVlPgo8L2RzOlJlZmVyZW5jZT4KPC9kczpTaWduZWRJbmZvPgo8ZHM6U2lnbmF0dXJlVmFsdWU+Clg3NEtrRllpU1ZsdE5KK1hBa2RQbEg5R0pXUEF5aG5FR2Y1K1ozdjVEYjF5MlhhNDMxUDVsSGRSVGFXQnFMb3ZRN0h5ejBFeGx1aXMKR1dhMEtKSndqa2NaL0w3dFdKSVZZS2dNaFFnNmpVTEYwVzJjdkdEaGxpZ2ROdzFGUWdkdFdEUmt3Q0JJMFU3SW5vU2l2cks2dTNaZApKZ0g5NFBLYmROQTdRb1YzOXNRPQo8L2RzOlNpZ25hdHVyZVZhbHVlPgo8ZHM6S2V5SW5mbz4KPGRzOlg1MDlEYXRhPgo8ZHM6WDUwOUNlcnRpZmljYXRlPgpNSUlFTmpDQ0E1NmdBd0lCQWdJVUVVY29kWUxmbTdSeERrWVlCS3pVbTRpL1JjUXdEZ1lLS29NT0F3b0JBUUlEQWdVQU1GMHhUakJNCkJnTlZCQU1NUmRLdzBKdlFvdENpMEt2U21pRFFtdENqMDVqUW05Q1EwSjNRbE5DcjBLRFFvOUNvMEtzZzBKN1FvTkNpMEpEUW05Q3IKMHBvZ0tFZFBVMVFwSUZSRlUxUWdNakF5TWpFTE1Ba0dBMVVFQmhNQ1Mxb3dIaGNOTWpNeE1UQTVNVEF4T0RRd1doY05NalF4TVRBNApNVEF4T0RRd1dqQjVNUjR3SEFZRFZRUUREQlhRb3RDVjBLSFFvdENlMEpJZzBLTFFsZENoMEtJeEZUQVRCZ05WQkFRTUROQ2kwSlhRCm9kQ2kwSjdRa2pFWU1CWUdBMVVFQlJNUFNVbE9NVEl6TkRVMk56ZzVNREV4TVFzd0NRWURWUVFHRXdKTFdqRVpNQmNHQTFVRUtnd1EKMEtMUWxkQ2gwS0xRbnRDUzBKalFwekNCckRBakJna3FndzREQ2dFQkFnSXdGZ1lLS29NT0F3b0JBUUlDQVFZSUtvTU9Bd29CQXdNRApnWVFBQklHQTJ2elNueEMvSzc0VjBiOVF6Si9yMm1NOUd4aGJPb3NZS0kwU0JDbkFvMkliZUE1SEt6emhsdDVQaEVlR0VkVE40d3I5CmJXK3hSNHZ4RUJWcURuaUFZUlI2dW5DZnpPbVJ5N25NbzV5QTRKUHd1ODcyNE9abHdlMkNFQnNtQnl0QXlSZUN0aDZSWk15ZUF6VEQKL1M2YXlvQm1LZlZBbENGQjlCWDVjRUpWVjJxamdnSEdNSUlCd2pBNEJnTlZIU0FFTVRBdk1DMEdCaXFERGdNREFqQWpNQ0VHQ0NzRwpBUVVGQndJQkZoVm9kSFJ3T2k4dmNHdHBMbWR2ZGk1cmVpOWpjSE13ZHdZSUt3WUJCUVVIQVFFRWF6QnBNQ2dHQ0NzR0FRVUZCekFCCmhoeG9kSFJ3T2k4dmRHVnpkQzV3YTJrdVoyOTJMbXQ2TDI5amMzQXZNRDBHQ0NzR0FRVUZCekFDaGpGb2RIUndPaTh2ZEdWemRDNXcKYTJrdVoyOTJMbXQ2TDJObGNuUXZibU5oWDJkdmMzUXlNREl5WDNSbGMzUXVZMlZ5TUVFR0ExVWRId1E2TURnd05xQTBvREtHTUdoMApkSEE2THk5MFpYTjBMbkJyYVM1bmIzWXVhM292WTNKc0wyNWpZVjluYjNOME1qQXlNbDkwWlhOMExtTnliREJEQmdOVkhTNEVQREE2Ck1EaWdOcUEwaGpKb2RIUndPaTh2ZEdWemRDNXdhMmt1WjI5MkxtdDZMMk55YkM5dVkyRmZaMjl6ZERJd01qSmZaRjkwWlhOMExtTnkKYkRBZEJnTlZIU1VFRmpBVUJnZ3JCZ0VGQlFjREJBWUlLb01PQXdNRUFRRXdEZ1lEVlIwUEFRSC9CQVFEQWdQSU1CMEdBMVVkRGdRVwpCQlNCUnloMWd0K2J0SEVPUmhnRXJOU2JpTDlGeERBZkJnTlZIU01FR0RBV2dCVDYwa3NibzZESllmNGNxRkErYXFLN1JRMjRvekFXCkJnWXFndzREQXdVRUREQUtCZ2dxZ3c0REF3VUJBVEFPQmdvcWd3NERDZ0VCQWdNQ0JRQURnWUVBYWMvdDVLYnhKU21YN01oMVd6bHEKbTgzVXVZNmlHbjJUcm5EVFROMG42b0Z1TkJUOWxGeXZYNkh3d1hGN0syY1YwQy9EL2JkMnZ2VHJMVlRkaWxkNnlxWCswcmY0bUh0TApmWkFvMldZdCs5TGV5YldwcEUyWkdrYXhYQ1MyZGkrYlp3eVBvVjJOaUEwUUpSZ1FBdHVDRHk3TFkrcHlTQk5VdHZCZFVYZ1ljeE09CjwvZHM6WDUwOUNlcnRpZmljYXRlPgo8L2RzOlg1MDlEYXRhPgo8L2RzOktleUluZm8+CjwvZHM6U2lnbmF0dXJlPjwvVGVuZGVyU2lnbkRhdGE+";
var xml = XmlEscape(Encoding.UTF8.GetString(Convert.FromBase64String(xmlb64)));

var certificate = GetCertificateFromXml(xml);

Console.WriteLine("Xml = " + xml);

// var flags = KalkanSignFlags.SignCms | KalkanSignFlags.DetachedData;

// var signedData = client.SignData(data, KalkanSignType.Cms, KalkanInputFormat.Pem, 
//     KalkanOutputFormat.Pem);

// var signedData = client.SignData(data, flags);

// Console.WriteLine(signedData);

// flags |= KalkanSignFlags.DoNotCheckCertificateTime;

try
{
    // StringBuilder sb = new StringBuilder(signedData);
    // sb[67] = 'C';
    // client.VerifyData(data, signedData, KalkanSignType.Cms, KalkanInputFormat.Pem, KalkanOutputFormat.Pem);

    // var cms = new SignedCms(SubjectIdentifierType.IssuerAndSerialNumber);
    // cms.Decode(ConvertPemToBytes(s));
    // var certificate = cms.SignerInfos[0].Certificate;

    client.LoadKeyStore(KalkanStorageType.X509Cert, certificate.RawData, string.Empty);

    /*client.LoadCertificateFromFile("/home/al/certs/root_gost.crt", KalkanCertificateType.CertificateAuthority);
    client.LoadCertificateFromFile("/home/al/certs/root_gost_2022.cer", KalkanCertificateType.CertificateAuthority);
    client.LoadCertificateFromFile("/home/al/certs/root_rsa.crt", KalkanCertificateType.CertificateAuthority);
    client.LoadCertificateFromFile("/home/al/certs/root_rsa_2020.cer", KalkanCertificateType.CertificateAuthority);

    client.LoadCertificateFromFile("/home/al/certs/nca_gost.cer", KalkanCertificateType.IntermediateCertificate);
    client.LoadCertificateFromFile("/home/al/certs/nca_rsa.cer", KalkanCertificateType.IntermediateCertificate);
    client.LoadCertificateFromFile("/home/al/certs/nca_gost_2022.cer", KalkanCertificateType.IntermediateCertificate);
    client.LoadCertificateFromFile("/home/al/certs/nca_rsa_2022.cer", KalkanCertificateType.IntermediateCertificate);

    client.LoadCertificateFromFile("/home/al/certs/nca_gost2022_test.pem", KalkanCertificateType.CertificateAuthority);
    client.LoadCertificateFromFile("/home/al/certs/root_test_gost_2022.pem", KalkanCertificateType.CertificateAuthority);
*/
    // client.VerifyData(data, s, KalkanSignType.Cms, KalkanInputFormat.Pem, KalkanOutputFormat.Pem);
    
    // client.VerifyData(data, s, flags);

    client.VerifyXml(xml, KalkanSignFlags.DoNotCheckCertificateTime);
    Console.WriteLine("Data verified successfully!");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

byte[] ConvertPemToBytes(string pem)
{
    return Convert.FromBase64String(pem.Replace("-----BEGIN CMS-----", string.Empty)
				.Replace("-----END CMS-----", string.Empty)
				.Replace("\r", string.Empty)
				.Replace("\n", string.Empty));
}

X509Certificate2 GetCertificateFromXml(string xml)
{
    var xmlDoc = new XmlDocument();
    xmlDoc.LoadXml(xml);

    var certificateNode = xmlDoc.GetElementsByTagName("ds:X509Certificate")[0];

    var base64Certificate = certificateNode.InnerText;
    return new X509Certificate2(Convert.FromBase64String(base64Certificate));
}

// [return: NotNullIfNotNull(nameof(s))]
static string? XmlEscape(string? s)
{
    if (string.IsNullOrEmpty(s))
        return s;

    return string.Join("", s.Select(c => c < 127 ? c.ToString() : "&#" + (short)c + ";"));
}