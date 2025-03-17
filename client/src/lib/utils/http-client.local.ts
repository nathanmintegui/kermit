/**
 * Trecho retirado do blog Felippe Regazio
 *
 * https://felipperegazio.com/posts/mocking-apis-on-front/
 *
 */

export const mockRequest = (data, options, sleep = 200) => {
	const response = new Response(JSON.stringify(data), options);

	return new Promise((resolve, reject) => {
		setTimeout(() => {
			options.status >= 400 ? reject(response) : resolve(response);
		}, sleep);
	});
};
