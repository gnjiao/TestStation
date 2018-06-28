function [k,detail] = weist2( radius, distance )
%radius=[4 2 1 2 4 ]';
%distance=[1 2 3 4 5 ]';

%sym t;
f=fittype('a*t*t + b*t + c','independent','t','coefficients',{'a','b','c'});
cfun = fit(distance,radius,f) %显示拟合函数，数据必须为列向量形式

%xi=0:0.1:20;
%yi=cfun(xi);

%figures
%plot(distance,radius,'r*',xi,yi,'b-');
%title('拟合函数图形');

k = (4*cfun.a*cfun.c - cfun.b*cfun.b) / (4*cfun.a)
detail = cfun;

end