<script lang="ts">
	import { page } from '$app/state';
	import ImgErro from '../assets/img/error.jpg';
</script>

<div class="flex flex-col items-center justify-center">
	<a href="/criar-calendario" class="absolute top-12 left-12 text-white">Voltar</a>
	<h1 class="bold text-6xl text-white">{page?.error?.message}</h1>
	<img src={ImgErro} alt="error" class="" />
</div>

<style>
	* {
		min-height: 100%;
		background-color: black;
	}
</style>
