var root_path = "/ScfDocs/cn/docs/doc";
var debug = false;

// 程序入口
$(function () {
	debugger
	if(debug){
		root_path = "";
	}
	initZoom();
	initHead();
})

// md文档解析器初始化
var md = window.markdownit({
	html: true
	// linkify: true,
	// typographer: true
});


// 获取锚点参数
function Hash (name, value) {
	var reg = new RegExp(name + '=(.*?)(&|$)');
	return location.hash.match(reg) && location.hash.match(reg)[1];
}

// 锚点变化处理，模拟 Router
function hashChange () {
	var sort = Hash('sort')
	var doc = Hash('doc')
	$('#readme').html('')
	$('#anchor').html('')
	$('#chapter-list').html('')
	if (sort && doc) {
		initReadMe(sort + '/' + doc)
		initAnchor(doc)
	} else if (sort && !doc) {
		initMenu(sort)
	}

}

window.onhashchange = hashChange

// 图片放大镜初始化
function initZoom () {
	var $zom = $('.zoom-img');
	var $zombtn = $('.source-btn');
	var $zomimg = $('.source-img');
	$('.content').delegate('img', 'click', function () {
		var src = $(this).attr('src');
		$zomimg.attr('src', src);
		$zombtn.attr('href', src);
		$zom.fadeIn();
	})
	$zomimg.click(function () {
		$zom.fadeOut();
	})
}

// 生成文章顶部位置导航位置，并自动展开左侧文章位置。
function initAnchor (doc) {
	$('.menu a').removeClass('hot')
	var $a = $('a[href="' + doc + '"]')
	// 避免刷新页面，左侧菜单并未初始化。
	if ($a.length === 0) {
		setTimeout(() => initAnchor(doc), 500)
	} else {
		// 默认展开左侧所在目录
		$a.addClass('hot').parents('li.fold').removeClass('fold')
		// 初始化展示文件所在目录地址
		var anchors = [$a.text()]
		$a.parents('ul').each(function () {
			anchors.unshift($(this).prev().text())
		})
		$('#anchor').html(anchors.join('  >  '))
	}
}

// 初始化右上角分类大链接跳转
function initHead() {
	$.get(root_path + '/SUMMARY.md', function (data) {
		var result = md.render(data);
		$('.header').append(result);
		$('.header ul').addClass('g-clearfix');
		$('.header a').click(function () {
			location.hash = 'sort=' + $(this).attr('href')
			return false
		})
		if (location.hash === '') {
			initMenu($('.header a').eq(0).attr('href'))
		} else if (Hash('sort')) {
			initMenu(Hash('sort'))
		}
		hashChange()
	})
}

// 初始化左侧菜单与点击事件
function initMenu(path) {
	$.get(root_path + '/' + path + '/SUMMARY.md', function (data) {
		var result = md.render(data);
		$('.menu').html(result);
		$('.menu a').click(function () {
			if ($(this).attr('href').indexOf('http://') > -1) {
				window.open($(this).attr('href'));
				return false
			}
			location.hash = 'sort=' + root_path + '/' + path + '&doc=' + $(this).attr('href')
			return false
		})
		$('.content').css('padding-left', $('.menu').width())
		$('#anchor').css('padding-left', $('.menu').width())
	})
}

// 初始化文章
function initReadMe (path) {
	$.get(path, function (data) {
		var result = md.render(data);

		result = autoImagePrefix(result)

		$('#readme').html(result);

		// 高亮代码块
		highLight();

		// md文档中内联文档链接处理
		// mdLink(Hash('sort'))

		// 需要延迟渲染右侧文章导航，才能计算滚动距离的精确位置。
		setTimeout(initChapterList, 600);
	})
}

// 生成文章右侧内容标题
function initChapterList () {
	$('#chapter-list').append('<div class="chapter-title">本文目录</div>')
	$('#readme h1, #readme h2').each(function () {
		var _top = $(this).offset().top
		var $item = $('<div class="chapter-item">' + $(this).text() + '</div>')
		$('#chapter-list').append($item)
		$item.click(function () {
			$('#readme').scrollTop( _top - 116 )
		})
	})
}

// 高亮文章代码部分
function highLight () {
	$('#readme pre code').each(function(i, block) {
		hljs.highlightBlock(block);
	});
}

// 采用绝对地址进行跳转，此处不用再做解析。比如：/#sort=dongjian&doc=chapter1/ios-ar-configuration.md
// function mdLink (sort) {
// 	$('#readme a[href$=".md"]').click(function () {
// 		location.hash = 'sort=' + sort + '&doc=' + $(this).attr('href')
// 		return false
// 	})
// }

// 校验图片引用是否为相对路径，如果是相对路径，则自动补充成绝对路径。
function autoImagePrefix (doc) {
	try {
		return doc.replace(/src=\"(.*?)\"/ig, function (match, path) {
			var sort = Hash('sort');
			if (path.indexOf(sort) === -1 && path.indexOf('http') === -1) {
				var doc = Hash('doc').split('/');
				doc.pop();
				doc.unshift(sort);
				doc.push(path);
				//return 'src="' + doc.join('/') + '"';
				return 'src="' + root_path + '/' + path + '"';
			}
			return match
		})
	} catch (e) {
		return doc
	}
}
