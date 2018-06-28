function [k,detail] = weist2( radius, distance )
%radius=[4 2 1 2 4 ]';
%distance=[1 2 3 4 5 ]';

%sym t;
f=fittype('a*t*t + b*t + c','independent','t','coefficients',{'a','b','c'});
cfun = fit(distance,radius,f) %��ʾ��Ϻ��������ݱ���Ϊ��������ʽ

%xi=0:0.1:20;
%yi=cfun(xi);

%figures
%plot(distance,radius,'r*',xi,yi,'b-');
%title('��Ϻ���ͼ��');

k = (4*cfun.a*cfun.c - cfun.b*cfun.b) / (4*cfun.a)
detail = cfun;

end