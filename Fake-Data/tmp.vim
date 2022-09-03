let SessionLoad = 1
let s:so_save = &g:so | let s:siso_save = &g:siso | setg so=0 siso=0 | setl so=-1 siso=-1
let v:this_session=expand("<sfile>:p")
silent only
silent tabonly
cd D:/ASP-dotNet-Projects/NtlSystem/Fake-Data
if expand('%') == '' && !&modified && line('$') <= 1 && getline(1) == ''
  let s:wipebuf = bufnr('%')
endif
let s:shortmess_save = &shortmess
set shortmess=aoO
badd +0 fake_order.py
badd +721 term://D:/ASP-dotNet-Projects/NtlSystem/Fake-Data//292:C:/Windows/system32/cmd.exe
badd +90 D:/Github/Intern_SQL/SQL/Javascript2SQL/genDummyData.js
badd +68 D:/Github/odoo_server/odoo/custom_addons/odoo_controller/manufacturing_order/connection.py
badd +1 D:/Github/Intern_SQL/SQL/NTL\ System/sql_queries/dummy_data.sql
badd +77 D:/Github/odoo_server/odoo/custom_addons/odoo_controller/sales_order/connection.py
badd +0 D:/Github/odoo_server/odoo/custom_addons/odoo_controller/delivery_order/connection.py
argglobal
%argdel
$argadd .
set stal=2
tabnew +setlocal\ bufhidden=wipe
tabnew +setlocal\ bufhidden=wipe
tabnew +setlocal\ bufhidden=wipe
tabrewind
edit fake_order.py
let s:save_splitbelow = &splitbelow
let s:save_splitright = &splitright
set splitbelow splitright
wincmd _ | wincmd |
vsplit
1wincmd h
wincmd w
let &splitbelow = s:save_splitbelow
let &splitright = s:save_splitright
wincmd t
let s:save_winminheight = &winminheight
let s:save_winminwidth = &winminwidth
set winminheight=0
set winheight=1
set winminwidth=0
set winwidth=1
exe 'vert 1resize ' . ((&columns * 31 + 106) / 213)
exe 'vert 2resize ' . ((&columns * 181 + 106) / 213)
argglobal
enew
file NERD_tree_7
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal nofen
wincmd w
argglobal
setlocal fdm=indent
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=2
setlocal fml=1
setlocal fdn=20
setlocal fen
140
normal! zc
176
normal! zc
226
normal! zo
let s:l = 228 - ((97 * winheight(0) + 23) / 46)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 228
normal! 025|
wincmd w
2wincmd w
exe 'vert 1resize ' . ((&columns * 31 + 106) / 213)
exe 'vert 2resize ' . ((&columns * 181 + 106) / 213)
tabnext
argglobal
if bufexists(fnamemodify("term://D:/ASP-dotNet-Projects/NtlSystem/Fake-Data//292:C:/Windows/system32/cmd.exe", ":p")) | buffer term://D:/ASP-dotNet-Projects/NtlSystem/Fake-Data//292:C:/Windows/system32/cmd.exe | else | edit term://D:/ASP-dotNet-Projects/NtlSystem/Fake-Data//292:C:/Windows/system32/cmd.exe | endif
if &buftype ==# 'terminal'
  silent file term://D:/ASP-dotNet-Projects/NtlSystem/Fake-Data//292:C:/Windows/system32/cmd.exe
endif
balt term://D:/ASP-dotNet-Projects/NtlSystem/Fake-Data//292:C:/Windows/system32/cmd.exe
setlocal fdm=indent
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
let s:l = 704 - ((13 * winheight(0) + 23) / 46)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 704
normal! 0
tabnext
edit D:/Github/odoo_server/odoo/custom_addons/odoo_controller/delivery_order/connection.py
let s:save_splitbelow = &splitbelow
let s:save_splitright = &splitright
set splitbelow splitright
wincmd _ | wincmd |
vsplit
1wincmd h
wincmd w
let &splitbelow = s:save_splitbelow
let &splitright = s:save_splitright
wincmd t
let s:save_winminheight = &winminheight
let s:save_winminwidth = &winminwidth
set winminheight=0
set winheight=1
set winminwidth=0
set winwidth=1
exe '1resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 1resize ' . ((&columns * 31 + 106) / 213)
exe '2resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 2resize ' . ((&columns * 154 + 106) / 213)
argglobal
enew
file NERD_tree_3
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal nofen
wincmd w
argglobal
balt D:/Github/odoo_server/odoo/custom_addons/odoo_controller/sales_order/connection.py
setlocal fdm=indent
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=2
setlocal fml=1
setlocal fdn=20
setlocal fen
let s:l = 44 - ((20 * winheight(0) + 15) / 30)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 44
normal! 0
wincmd w
exe '1resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 1resize ' . ((&columns * 31 + 106) / 213)
exe '2resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 2resize ' . ((&columns * 154 + 106) / 213)
tabnext
edit D:/Github/Intern_SQL/SQL/NTL\ System/sql_queries/dummy_data.sql
let s:save_splitbelow = &splitbelow
let s:save_splitright = &splitright
set splitbelow splitright
wincmd _ | wincmd |
vsplit
wincmd _ | wincmd |
vsplit
2wincmd h
wincmd w
wincmd w
let &splitbelow = s:save_splitbelow
let &splitright = s:save_splitright
wincmd t
let s:save_winminheight = &winminheight
let s:save_winminwidth = &winminwidth
set winminheight=0
set winheight=1
set winminwidth=0
set winwidth=1
exe '1resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 1resize ' . ((&columns * 31 + 106) / 213)
exe '2resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 2resize ' . ((&columns * 77 + 106) / 213)
exe '3resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 3resize ' . ((&columns * 76 + 106) / 213)
argglobal
enew
file NERD_tree_2
setlocal fdm=manual
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal nofen
wincmd w
argglobal
balt D:/Github/Intern_SQL/SQL/Javascript2SQL/genDummyData.js
setlocal fdm=indent
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
let s:l = 17 - ((4 * winheight(0) + 15) / 30)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 17
normal! 0
wincmd w
argglobal
if bufexists(fnamemodify("D:/Github/Intern_SQL/SQL/Javascript2SQL/genDummyData.js", ":p")) | buffer D:/Github/Intern_SQL/SQL/Javascript2SQL/genDummyData.js | else | edit D:/Github/Intern_SQL/SQL/Javascript2SQL/genDummyData.js | endif
if &buftype ==# 'terminal'
  silent file D:/Github/Intern_SQL/SQL/Javascript2SQL/genDummyData.js
endif
balt D:/Github/Intern_SQL/SQL/Javascript2SQL/genDummyData.js
setlocal fdm=indent
setlocal fde=0
setlocal fmr={{{,}}}
setlocal fdi=#
setlocal fdl=0
setlocal fml=1
setlocal fdn=20
setlocal fen
89
normal! zo
let s:l = 90 - ((9 * winheight(0) + 15) / 30)
if s:l < 1 | let s:l = 1 | endif
keepjumps exe s:l
normal! zt
keepjumps 90
normal! 0
wincmd w
exe '1resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 1resize ' . ((&columns * 31 + 106) / 213)
exe '2resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 2resize ' . ((&columns * 77 + 106) / 213)
exe '3resize ' . ((&lines * 30 + 25) / 50)
exe 'vert 3resize ' . ((&columns * 76 + 106) / 213)
tabnext 1
set stal=1
if exists('s:wipebuf') && len(win_findbuf(s:wipebuf)) == 0 && getbufvar(s:wipebuf, '&buftype') isnot# 'terminal'
  silent exe 'bwipe ' . s:wipebuf
endif
unlet! s:wipebuf
set winheight=1 winwidth=20
let &shortmess = s:shortmess_save
let &winminheight = s:save_winminheight
let &winminwidth = s:save_winminwidth
let s:sx = expand("<sfile>:p:r")."x.vim"
if filereadable(s:sx)
  exe "source " . fnameescape(s:sx)
endif
let &g:so = s:so_save | let &g:siso = s:siso_save
set hlsearch
doautoall SessionLoadPost
unlet SessionLoad
" vim: set ft=vim :
