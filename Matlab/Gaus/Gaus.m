function [k,detail] = Gaus( radius, pixelSum, ratio )

target = pixelSum(end)*ratio

ft = fittype( 'gauss3' );
opts = fitoptions( 'Method', 'NonlinearLeastSquares' );
opts.Display = 'Off';
%opts.Lower = [-Inf -Inf 0];
%opts.StartPoint = [y(ceil(length(x)/2)) x(ceil(length(x)/2)) 0.1]; % Fit model to data.
[cfun, gof] = fit( pixelSum, radius, ft, opts ); % Plot fit with data.

%si = 10000:5000:250644;
%ri = cfun(si)
%figure
%plot(radius,pixelSum,'r*',ri,si,'b-');

k = cfun(target)
detail = cfun

end