function [w,z,detail] = weist( x, y )

%sym t;
f=fittype('a*sqrt(1 + (t/b)^2)','independent','t','coefficients',{'a','b'});
cfun = fit(x,y,f);

%xi=-5:0.2:5;
%yi=cfun(xi);
%figure
%plot(x,y,'r*',xi,yi,'b-');
%title('Output Figure');

w = cfun.a;
z = cfun.b;
detail = cfun;

end
