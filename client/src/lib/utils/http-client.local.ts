export const mockRequest = async <T>(
	data: T,
	options: ResponseInit,
	sleep = 200
): Promise<T> => {
	const response = new Response(JSON.stringify(data), options);

	return new Promise((resolve, reject) => {
		setTimeout(() => {
			if (options.status && options.status >= 400) {
				reject(response);
				return;
			}

			response.json().then(resolve).catch(reject);
		}, sleep);
	});
};

