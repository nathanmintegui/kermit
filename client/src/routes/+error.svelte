<script lang="ts">
	import { page } from '$app/state';
	import ImgErro from '../assets/img/error.jpg';
</script>

<div class="flex flex-col justify-center items-center">
	<a href="/criar-calendario" class="text-white absolute top-12 left-12">Voltar</a>
	<h1 class="text-white bold text-6xl">{page?.error?.message}</h1>
	<img src={ImgErro} alt="error" class="" />
</div>

<style>
    * {
        min-height: 100%;
        background-color: black;
    }
</style>
